using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jint;

namespace EMacro
{
    internal class EMacroJsEngine
    {
        private static Jint.Engine engine = null;

        public static Jint.Engine GetJsEngine()
        {
            if (engine != null)
            {
                return engine;
            }
            else
            {
                engine = new Jint.Engine(cfg => cfg.AllowClr());
                InitJsEngine(engine);
                return engine;
            }
        }

        private static void InitJsEngine(Jint.Engine engine)
        {
            StringBuilder sb = new StringBuilder();
            foreach(string strFunc in listFunc)
            {
                sb.Append(strFunc);
            }
            engine.Execute(sb.ToString());
            engine.SetValue("colorFromName", new Func<string, System.Drawing.Color>(System.Drawing.Color.FromName));
            engine.SetValue("colorFromArgb", new Func<int, int, int, int, System.Drawing.Color>(System.Drawing.Color.FromArgb));
            engine.SetValue("colorFromRgb", new Func<int, int, int, System.Drawing.Color>(System.Drawing.Color.FromArgb));
        }

        private static List<string> listFunc = new List<string>()
        {
            @"function range(/*start,end*/){
                            var start = 0;
                            var end = 0;
                            if(arguments.length == 1){
                                end = arguments[0];
                            }else{
                                start = arguments[0];
                                end = arguments[1];
                            }
                            var result = []
                            for(var i=start;i<end;i++){
                                result.push(i);
                            }
                            return result;
            }",
            @"function color(){
                if(arguments.length == 1){
                    return colorFromName(arguments[0]);
                }else if(arguments.length == 3){
                    return colorFromRgb(arguments[0],arguments[1],arguments[2]);
                }else{
                    return colorFromArgb(arguments[0],arguments[1],arguments[2],arguments[3]);
                }
            }",
            @"function choose(value){
                if(arguments.length <= value + 1) return null;
                return arguments[value + 1];
            }
            "
        };

    }
}
