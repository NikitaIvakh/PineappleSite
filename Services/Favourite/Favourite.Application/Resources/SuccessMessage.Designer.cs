﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Favourite.Application.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SuccessMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SuccessMessage() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Favourite.Application.Resources.SuccessMessage", typeof(SuccessMessage).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ваша избранная продукция.
        /// </summary>
        internal static string FavouriteProducts {
            get {
                return ResourceManager.GetString("FavouriteProducts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Продукции успшено удалены.
        /// </summary>
        internal static string ProductsSuccessfullyDeleted {
            get {
                return ResourceManager.GetString("ProductsSuccessfullyDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Продукция успешно добавлена в избранное.
        /// </summary>
        internal static string ProductSuccessfullyAddedToFavourite {
            get {
                return ResourceManager.GetString("ProductSuccessfullyAddedToFavourite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Продукция успешно удалена.
        /// </summary>
        internal static string ProductSuccessfullyDeleted {
            get {
                return ResourceManager.GetString("ProductSuccessfullyDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В избранном никакой продукции нет.
        /// </summary>
        internal static string ThereAreNoFavouriteProducts {
            get {
                return ResourceManager.GetString("ThereAreNoFavouriteProducts", resourceCulture);
            }
        }
    }
}
