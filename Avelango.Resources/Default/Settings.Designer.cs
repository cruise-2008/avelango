﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Avelango.Resources.Default {
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
    public class Settings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Settings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Avelango.Resources.Default.Settings", typeof(Settings).Assembly);
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
        ///   Looks up a localized string similar to /Addmin/Parlour.
        /// </summary>
        public static string AdminParlourPath {
            get {
                return ResourceManager.GetString("AdminParlourPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /Storage/Avatars/defaultuser.png.
        /// </summary>
        public static string DefaulLogoPath {
            get {
                return ResourceManager.GetString("DefaulLogoPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /.
        /// </summary>
        public static string MainPath {
            get {
                return ResourceManager.GetString("MainPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /Modderator/Parlour.
        /// </summary>
        public static string ModeratorParlourPath {
            get {
                return ResourceManager.GetString("ModeratorParlourPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /Task/Tasks.
        /// </summary>
        public static string TasksPath {
            get {
                return ResourceManager.GetString("TasksPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /MyUser/Parlour.
        /// </summary>
        public static string UserParlourPath {
            get {
                return ResourceManager.GetString("UserParlourPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /Users/Executors.
        /// </summary>
        public static string UsersPath {
            get {
                return ResourceManager.GetString("UsersPath", resourceCulture);
            }
        }
    }
}
