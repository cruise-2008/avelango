﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Avelango.Resources.Causes.Ua {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Content {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Content() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Avelango.Resources.Causes.Ua.Content", typeof(Content).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Невірний формат даних.
        /// </summary>
        public static string BadDataFormat {
            get {
                return ResourceManager.GetString("BadDataFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Замість вашого фото опублікувано фотографії тварин, предметів, знаменитостей.
        /// </summary>
        public static string IncorrectFoto {
            get {
                return ResourceManager.GetString("IncorrectFoto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Непристойний зміст .
        /// </summary>
        public static string ObscenityPublishing {
            get {
                return ResourceManager.GetString("ObscenityPublishing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Підозра, що Ви бот.
        /// </summary>
        public static string SuspicionBot {
            get {
                return ResourceManager.GetString("SuspicionBot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Порушення правил.
        /// </summary>
        public static string ViolationRules {
            get {
                return ResourceManager.GetString("ViolationRules", resourceCulture);
            }
        }
    }
}