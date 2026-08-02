use std::{env, fs::{self, File, OpenOptions}, io::Write};
use xmltree::{Element, EmitterConfig, XMLNode};
use crate::structure::{PatchArgs, PatchType};

pub fn add_file(file: String, instance_class: bool, is_public: bool) {
    let mut full_path: Vec<String> = file.split(".").map(|x| x.to_string()).collect();
    if let Some(class) = full_path.pop() {
        let namespace = full_path.join(".");

        let mut class_decl = vec![if is_public {"public"} else { "internal"}];
        if !instance_class { class_decl.push("static"); }
        class_decl.push("class");
        class_decl.push(&class);
        
        let mut text = vec![
            format!("namespace {namespace} {{"),
            format!("    {} {{", class_decl.join(" ")),
            format!("    }}"),
            format!("}}")
        ].join("\n").into_bytes();

        let project = get_project_root();
        save(&full_path, &class, &project, &mut text);

        let assembly = full_path.get(0).expect("Failed to retrieve assembly name");
        let path = &full_path[1..].join("\\");
        
        update_proj(assembly, path, &class, &project);
    }
}

pub fn add_patch(args: PatchArgs, patch_type: PatchType) {
    let mut full_path: Vec<String> = args.file.split(".").map(|x| x.to_string()).collect();

    if let Some(class) = full_path.pop() {
        let namespace = full_path.join(".");
        let (method, return_type) = match patch_type {
            PatchType::Prefix => (format!("Prefix({} __instance)", args.target_class), "bool"),
            PatchType::Postfix => (format!("Postfix({} __instance)", args.target_class), "void"),
            PatchType::Transpiler => (format!("Transpiler({} __instance, IEnumerable<CodeInstruction> instructions, ILGenerator generator)", args.target_class), "IEnumerable<CodeInstruction>"),
        };

        let transpiler_includes = if let PatchType::Transpiler = patch_type {
            vec![
                "using System.Collections.Generic;",
                "using System.Reflection.Emit;"
            ]
            .into_iter()
            .map(|x| x.to_string())
            .collect::<Vec<_>>()
        } else { vec![] };

        let text = vec![
            format!("using HarmonyLib;\n"),
            format!("namespace {namespace} {{"),
            format!("    [HarmonyPatch(typeof({}), \"{}\")]", args.target_class, args.target_method),
            format!("    public static class {class} {{"),
            format!("        public static {return_type} {method} {{"),
            format!("        }}"),
            format!("    }}"),
            format!("}}")
        ];
        let mut text = [transpiler_includes, text]
            .concat()
            .join("\n")
            .into_bytes();

        let project = get_project_root();
        save(&full_path, &class, &project, &mut text);

        let assembly = full_path.get(0).expect("Failed to retrieve assembly name");
        let path = &full_path[1..].join("\\");
        
        update_proj(assembly, path, &class, &project);
    }
}

fn get_project_root() -> String {
    let user_root = env::var("USERPROFILE").expect("Failed to read user folder root");
    user_root + "/source/repos/SplasherArchipelago"
}

fn save(dir_path: &[String], class: &str, project_root: &str, text: &mut [u8]) {
    let dir_path = format!("{project_root}/{}", dir_path.join("/"));
    if !dir_path.is_empty() {
        fs::create_dir_all(&dir_path).unwrap();
    }

    let filepath = if dir_path.is_empty() {
        format!("{}.cs", class)
    } else {
        format!("{}/{}.cs", dir_path, class)
    };
    
    let mut file = File::create(filepath).unwrap();
    file.write_all(text).unwrap();
}

fn update_proj(assembly: &str, path: &str, class: &str, project_root: &str) {
    let proj_path = format!("{project_root}/{assembly}/{assembly}.csproj");
    let file = File::open(&proj_path).unwrap();

    let mut root  = Element::parse(&file).unwrap();
    let mut target_group = None;

    root.children.iter_mut().for_each(|node| {
        if 
            let XMLNode::Element(el) = node && 
            el.name == "ItemGroup" &&
            el.children.iter().any(|x| {
                match x {
                    XMLNode::Element(child) => child.name == "Compile",
                    _ => false
                }
            } && !el.children.iter().any(|x| {
                match x {
                    XMLNode::Element(child) => child.attributes.values().any(|v| {
                        v.contains(class)
                    }),
                    _ => false
                }
            })
        ) {
            target_group = Some(el)
        }
    });

    if let Some(group) = target_group {
        let path = path.replace("/", "\\");
        let mut new_compile = Element::new("Compile");
        new_compile.attributes.insert("Include".to_string(), format!("{path}\\{class}.cs"));
        new_compile.namespace = group.namespace.clone();

        group.children.push(XMLNode::Element(new_compile));
    }

    let mut conf = EmitterConfig::new();
    conf.perform_indent = true;

    let mut file = OpenOptions::new()
        .write(true)
        .truncate(true)
        .open(&proj_path)
        .unwrap();
    root.write_with_config(&mut file, conf).unwrap();

    let mut output = fs::read_to_string(&proj_path)
        .unwrap()
        .replace("&apos;", "'")
        .into_bytes();

    let mut file = OpenOptions::new()
        .write(true)
        .truncate(true)
        .open(&proj_path)
        .unwrap();
    file.write_all(&mut output).unwrap();
}