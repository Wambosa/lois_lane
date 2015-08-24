using System;
using Novacode;
using Mono.Options;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LoisLane {

    class Program {

        delegate void ReportGenerator(string input, string reportFilePath);

        private enum REPORT_TYPE {NONE, DESIGN_DOCUMENT};

        private static REPORT_TYPE THIS_REPORT = REPORT_TYPE.NONE;

        private static Dictionary<REPORT_TYPE, ReportGenerator> UseTemplate = new Dictionary<REPORT_TYPE, ReportGenerator>() {
            {REPORT_TYPE.NONE, EchoHelp},
            {REPORT_TYPE.DESIGN_DOCUMENT, WriteDesignDocument},
        };

        public static void Main(string[] args) {
            
            /* either serialized json in a file or as the raw argument */
            string fileName = null;
            string rawJson = null;
            string outFile = GetThisFolder() + "new_file.docx"; //todo: deal with other extentions later

            var options = new OptionSet() {
                { "design-document", "", _ => {THIS_REPORT = REPORT_TYPE.DESIGN_DOCUMENT;}},

                { "?|h|help", "", _ => Echo.HelpText()},

                {"json=|raw-json=", "", input => {rawJson = input;}},
                {"file=|file-path=", "", input => {fileName = input;}},
                {"out=|out-file=", "", input => {outFile = input;}},
                
                {"l=|log-level=", "", noise => {Echo.LOG_LEVEL = Convert.ToByte(noise);}},
            };

            var badInput = options.Parse(args);

            if(badInput.Count > 0) {
                Echo.ErrorReport(badInput.ToArray());
                Echo.HelpText();
            }

            Echo.WelcomeText();

            UseTemplate[THIS_REPORT](GetJson(fileName) ?? rawJson, outFile);

            Echo.Out("done", 5);
        }

        private static void WriteDesignDocument(string rawJson, string outputFilePath){

            System.IO.File.Copy(GetThisFolder() + "Templates\\Design_Template.docx", outputFilePath, true);

            JObject json = JObject.Parse(rawJson);

            using (DocX document = DocX.Load(outputFilePath)){ //this might be a path issue

                foreach(var property in json){
                    document.ReplaceText("%"+property.Key.ToUpper()+"%", property.Value.ToString());}

               document.Save();
            }//dalloc

            
            //some properties will not exist in the template becasue they need special handling.
            //do some special array handling and formatting here.
            // use a keyword _list or _otherword to trigger special processing

        }

        private static void EchoHelp(string ignore, string ignored){
            Echo.HelpText();
        }

        private static string GetJson(string inputFilePath){
            string returnJson = null;
            try{returnJson = System.IO.File.ReadAllText(inputFilePath);}catch{}
            return returnJson;
        }

        private static string GetThisFolder() {
            var path_bits = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');

            string simple_path_i_have_to_build_because_you_suck_microsoft = "";

            for(int i=0; i<(path_bits.Length-1); ++i) {
                simple_path_i_have_to_build_because_you_suck_microsoft += path_bits[i]+'\\';
            }

            return simple_path_i_have_to_build_because_you_suck_microsoft;
        }
    }
}