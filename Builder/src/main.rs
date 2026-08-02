use clap::Parser;
use structure::{Cli, Command};

use crate::add::{add_file, add_patch};

mod add;
mod structure;



fn main() {
    let command = Cli::parse().command;
    match command {
        Command::Add { file, instance, public } => add_file(file, instance, public),
        Command::AddPrefix( args ) => add_patch(args, structure::PatchType::Prefix),
        Command::AddPostfix(args) => add_patch(args, structure::PatchType::Postfix),
        Command::AddTranspiler(args) => add_patch(args, structure::PatchType::Transpiler),
    }
}
