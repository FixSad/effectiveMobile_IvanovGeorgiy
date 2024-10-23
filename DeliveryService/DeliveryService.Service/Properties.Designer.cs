namespace DeliveryService.Service {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.11.0.0")]
    internal sealed partial class Properties : global::System.Configuration.ApplicationSettingsBase {
        
        private static Properties defaultInstance = ((Properties)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Properties())));
        
        public static Properties Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DeliveryOrder.txt")]
        public string DeliveryOrder {
            get {
                return ((string)(this["DeliveryOrder"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DeliveryLog.txt")]
        public string DeliveryLog {
            get {
                return ((string)(this["DeliveryLog"]));
            }
            set {
                this["DeliveryLog"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AllOrders.txt")]
        public string AllOrders {
            get {
                return ((string)(this["AllOrders"]));
            }
        }
    }
}
