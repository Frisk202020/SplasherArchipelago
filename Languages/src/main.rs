use std::{collections::HashMap, fs, io::Write};
use xmlwriter::{Indent::None, Options, XmlWriter};

fn to_xml(filename: String, content: HashMap<String, String>) -> Result<(String, String), String> {
    let mut w = XmlWriter::new(Options { indent: None, ..Options::default() });
    w.write_declaration();

    w.start_element("LanguageCategoryEntry");
    w.write_attribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
    w.write_attribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

    w.start_element("ContainerFile");
    w.write_text(&filename);
    w.end_element();
    
    w.start_element("Entries");
    for x in content {
        w.start_element("item");

        for (label, value) in [("key", &x.0), ("value", &x.1)] {
            w.start_element(label);
            w.start_element("string");
            w.write_text(value);
            w.end_element();
            w.end_element();
        }

        w.end_element();
    }

    Ok((filename, w.end_document()))
}

fn main() -> Result<(), String> {
    let outputs = fs::read_dir("input").map_err(|e| format!("Failed to read input directory : {e:?}"))?
        .map(|x| -> Result<_, String> {
            let entry = x.map_err(|e| format!("Failed to parse a file path : {e:?}"))?;
            let content = fs::read_to_string(entry.path()).map_err(|e| format!("Failed to parse a file : {e:?}"))?;
            let filename = entry.file_name()
                .into_string()
                .map_err(|e| format!("Failed to parse this file name : {e:?}"))?;

            let mut filename_split = filename.split('.');
            let extract = (filename_split.next(), filename_split.next());

            match extract {
                (Some(category), Some(lang)) => Ok((category.to_string(), lang.to_string(), content)),
                _ => Err(format!("Unexpected file name : {filename}. Make sure to format it as <CATEGORY>.<LANGUAGE>."))
            }
        }).collect::<Result<Vec<(String, String, String)>, String>>()?
        .into_iter()
        .map(|x| -> Result<_, String> {
            let content = serde_json::from_str::<HashMap<String, String>>(&x.2).map_err(|e| format!("Failed to parse JSON in {} : {e:?}", x.0))?;
            Ok((x.0, x.1, content))
        }).collect::<Result<Vec<(String, String, HashMap<String, String>)>, String>>()?
        .into_iter()
        .map(|x| -> Result<(_,_,_), String>{ 
            let (filename, content) = to_xml(x.0,x.2)?;
            Ok((filename, x.1, content))
        })
        .collect::<Result<Vec<(String, String, String)>, String>>()?;

    for (filename, lang, content) in outputs {
        let mut file = fs::OpenOptions::new()
            .write(true)
            .create(true)
            .truncate(true)
            .open(format!("C:/Program Files (x86)/Steam/steamapps/common/Splasher/Splasher_Data/StreamingAssets/Languages/{}.{lang}.txt", filename))
            .map_err(|e| format!("Failed to open or create output file {filename} : {e:?}"))?;

        file.write_all(content.as_bytes()).map_err(|e| format!("Failed to write output {filename} : {e:?}"))?;
    }

    Ok(())
}
