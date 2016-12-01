﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tavisca.Frameworks.Logging.Resources {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///    A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class LogResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        internal LogResources() {
        }
        
        /// <summary>
        ///    Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tavisca.Frameworks.Logging.Core.Resources.LogResources", typeof(LogResources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///    Overrides the current thread's CurrentUICulture property for all
        ///    resource lookups using this strongly typed resource class.
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
        ///    Looks up a localized string similar to The compression type provided in the configuration &apos;{0}&apos; is not supported, contact administrator..
        /// </summary>
        public static string CompressionType_NotSupported {
            get {
                return ResourceManager.GetString("CompressionType_NotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The custom locator type: {0} could not be resolved..
        /// </summary>
        public static string Configuration_CouldNotResolveCustomLocator {
            get {
                return ResourceManager.GetString("Configuration_CouldNotResolveCustomLocator", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The customCompressionType provided in the configuration must implement ICompressionProvider.
        /// </summary>
        public static string Configuration_CustomCompressionType_Invalid {
            get {
                return ResourceManager.GetString("Configuration_CustomCompressionType_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The customCompressionType provided in the configuration cannot be found.
        /// </summary>
        public static string Configuration_CustomCompressionType_NotFound {
            get {
                return ResourceManager.GetString("Configuration_CustomCompressionType_NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The CustomFormatter provided in the configuration must implement ILogEntryFormatter.
        /// </summary>
        public static string Configuration_CustomFormatter_Invalid {
            get {
                return ResourceManager.GetString("Configuration_CustomFormatter_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The CustomFormatter provided in the configuration cannot be found..
        /// </summary>
        public static string Configuration_CustomFormatter_NotFound {
            get {
                return ResourceManager.GetString("Configuration_CustomFormatter_NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The CustomLocatorAdapter must implement &quot;Microsoft.Practices.ServiceLocation.IServiceLocator&quot;.
        /// </summary>
        public static string Configuration_IncorrectCustomLocatorProvider {
            get {
                return ResourceManager.GetString("Configuration_IncorrectCustomLocatorProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to A locator adapter must be configured in the system..
        /// </summary>
        public static string Configuration_MissingCustomLocatorProvider {
            get {
                return ResourceManager.GetString("Configuration_MissingCustomLocatorProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to A default logger was required but not found in the application, please configure a default logger or else check the category passed, The category passed was {0}..
        /// </summary>
        public static string DefaultLogger_NotFound {
            get {
                return ResourceManager.GetString("DefaultLogger_NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to An error occurred while logging data into the system, see inner exception for details.
        /// </summary>
        public static string GenericLogExceptionMessage {
            get {
                return ResourceManager.GetString("GenericLogExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to An unknown error has occurred. See inner exception for more details..
        /// </summary>
        public static string LogConfigurationError_Generic {
            get {
                return ResourceManager.GetString("LogConfigurationError_Generic", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to An error occurred while creating an instance of the LogConfigurationProvider specified in the configuration. the type specified was {0}. See inner exception for further details..
        /// </summary>
        public static string LogConfigurationProvider_InstanceCreation {
            get {
                return ResourceManager.GetString("LogConfigurationProvider_InstanceCreation", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The custom LogConfigurationProvider did not return any configuration, kindly check implementation..
        /// </summary>
        public static string LogConfigurationProvider_MissingConfiguration {
            get {
                return ResourceManager.GetString("LogConfigurationProvider_MissingConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The LogConfigurationProvider specified in the configuration was not found, the type specified was {0}..
        /// </summary>
        public static string LogConfigurationProvider_NotFound {
            get {
                return ResourceManager.GetString("LogConfigurationProvider_NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to Configuration section ApplicationLog is not configured..
        /// </summary>
        public static string MissingConfiguration {
            get {
                return ResourceManager.GetString("MissingConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The Service locator was not set, this can be due to a bug in the system, please contact administrator..
        /// </summary>
        public static string ServiceLocatorProvider_NotSet {
            get {
                return ResourceManager.GetString("ServiceLocatorProvider_NotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The provider returned a null value..
        /// </summary>
        public static string ServiceLocatorProviderDelegate_NullReturn {
            get {
                return ResourceManager.GetString("ServiceLocatorProviderDelegate_NullReturn", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The type {0} was unexpected, expected type was {1}.
        /// </summary>
        public static string UnexpectedObjectType {
            get {
                return ResourceManager.GetString("UnexpectedObjectType", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to The log entry type {0} is not supported by the system..
        /// </summary>
        public static string UnSupportedLogEntryType {
            get {
                return ResourceManager.GetString("UnSupportedLogEntryType", resourceCulture);
            }
        }
    }
}
