use clap::{Parser, Subcommand, Args};

#[derive(Parser)]
pub struct Cli {
    #[command(subcommand)]
    pub command: Command
}

#[derive(Args)]
pub struct PatchArgs {
    pub file: String,
    pub target_class: String,
    pub target_method: String,
}

pub enum PatchType {
    Prefix,
    Postfix,
    Transpiler
}

#[derive(Subcommand)]
pub enum Command {
    Add {
        file: String,
        #[arg(long)]
        public: bool,
        #[arg(long)]
        instance: bool,
    },
    AddPrefix(PatchArgs),
    AddPostfix(PatchArgs),
    AddTranspiler(PatchArgs),
}