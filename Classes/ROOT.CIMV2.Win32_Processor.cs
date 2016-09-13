namespace ContecAI.ROOT.CIMV2 {
    using System;
    using System.ComponentModel;
    using System.Management;
    using System.Collections;
    using System.Globalization;
    using System.ComponentModel.Design.Serialization;
    using System.Reflection;
    
    
    // Las funciones ShouldSerialize<PropertyName> son funciones que utiliza el Examinador de propiedades de VS para comprobar si se tiene que serializar una propiedad determinada. Estas funciones se agregan para todas las propiedades ValueType (propiedades de tipo Int32, BOOL etc. que no se pueden establecer como NULL). Estas funciones utilizan la función Is<PropertyName>Null. Asimismo, se utilizan en la implementación de TypeConverter para que las propiedades comprueben el valor NULL de una propiedad, de modo que se pueda mostrar un valor vacío en el Examinador de propiedades si se utiliza la función de arrastrar y colocar en Visual Studio.
    // Las funciones Is<PropertyName>Null() se utilizan para comprobar si una propiedad tiene valores NULL.
    // Las funciones Reset<PropertyName> se agregan para las propiedades Nullable Read/Write. El diseñador de VS utiliza estas funciones en el Examinador de propiedades para establecer una propiedad como NULL.
    // Todas las propiedades que se agregan a la clase de la propiedad WMI tienen atributos establecidos para definir su comportamiento en el diseñador de Visual Studio, así como para definir el elemento TypeConverter que se debe utilizar.
    // Las funciones Datetime de conversión ToDateTime y ToDmtfDateTime se agregan a la clase para convertir la fecha y hora DMTF a System.DateTime y viceversa.
    // Se generó una clase Early Bound para la clase WMI.Win32_Processor
    public class Processor : System.ComponentModel.Component {
        
        // Propiedad privada que contiene el espacio de nombres WMI en el que reside la clase.
        private static string CreatedWmiNamespace = "ROOT\\CIMV2";
        
        // Propiedad privada que mantiene el nombre de la clase WMI, que creó esta clase.
        private static string CreatedClassName = "Win32_Processor";
        
        // Variable miembro privada que contiene el valor ManagementScope que utilizan los diferentes métodos.
        private static System.Management.ManagementScope statMgmtScope = null;
        
        private ManagementSystemProperties PrivateSystemProperties;
        
        // Objeto lateBound de WMI subyacente.
        private System.Management.ManagementObject PrivateLateBoundObject;
        
        // Variable miembro que almacena el comportamiento de 'confirmación automática' de la clase.
        private bool AutoCommitProp;
        
        // Variable privada que contiene la propiedad incrustada que representa la instancia.
        private System.Management.ManagementBaseObject embeddedObj;
        
        // Objeto WMI actual utilizado
        private System.Management.ManagementBaseObject curObj;
        
        // Etiqueta para indicar si la instancia es un objeto incrustado.
        private bool isEmbedded;
        
        // A continuación se muestran las diferentes sobrecargas de constructores para inicializar una instancia de la clase con un objeto WMI.
        public Processor() {
            this.InitializeObject(null, null, null);
        }
        
        public Processor(string keyDeviceID) {
            this.InitializeObject(null, new System.Management.ManagementPath(Processor.ConstructPath(keyDeviceID)), null);
        }
        
        public Processor(System.Management.ManagementScope mgmtScope, string keyDeviceID) {
            this.InitializeObject(((System.Management.ManagementScope)(mgmtScope)), new System.Management.ManagementPath(Processor.ConstructPath(keyDeviceID)), null);
        }
        
        public Processor(System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(null, path, getOptions);
        }
        
        public Processor(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path) {
            this.InitializeObject(mgmtScope, path, null);
        }
        
        public Processor(System.Management.ManagementPath path) {
            this.InitializeObject(null, path, null);
        }
        
        public Processor(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(mgmtScope, path, getOptions);
        }
        
        public Processor(System.Management.ManagementObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                PrivateLateBoundObject = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
                curObj = PrivateLateBoundObject;
            }
            else {
                throw new System.ArgumentException("El nombre de clase no coincide.");
            }
        }
        
        public Processor(System.Management.ManagementBaseObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                embeddedObj = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(theObject);
                curObj = embeddedObj;
                isEmbedded = true;
            }
            else {
                throw new System.ArgumentException("El nombre de clase no coincide.");
            }
        }
        
        // La propiedad devuelve el espacio de nombres de la clase WMI.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace {
            get {
                return "ROOT\\CIMV2";
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName {
            get {
                string strRet = CreatedClassName;
                if ((curObj != null)) {
                    if ((curObj.ClassPath != null)) {
                        strRet = ((string)(curObj["__CLASS"]));
                        if (((strRet == null) 
                                    || (strRet == string.Empty))) {
                            strRet = CreatedClassName;
                        }
                    }
                }
                return strRet;
            }
        }
        
        // Propiedad que señala a un objeto incrustado para obtener las propiedades System del objeto WMI.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementSystemProperties SystemProperties {
            get {
                return PrivateSystemProperties;
            }
        }
        
        // Propiedad que devuelve el objeto lateBound subyacente.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementBaseObject LateBoundObject {
            get {
                return curObj;
            }
        }
        
        // ManagementScope del objeto.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementScope Scope {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Scope;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    PrivateLateBoundObject.Scope = value;
                }
            }
        }
        
        // Propiedad que muestra el comportamiento de confirmación del objeto WMI. Si se establece como true, el objeto WMI se guarda automáticamente después de modificar cada propiedad. Por ejemplo: se llama a Put() después de modificar una propiedad.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit {
            get {
                return AutoCommitProp;
            }
            set {
                AutoCommitProp = value;
            }
        }
        
        // ManagementPath del objeto WMI subyacente.
        [Browsable(true)]
        public System.Management.ManagementPath Path {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Path;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    if ((CheckIfProperClass(null, value, null) != true)) {
                        throw new System.ArgumentException("El nombre de clase no coincide.");
                    }
                    PrivateLateBoundObject.Path = value;
                }
            }
        }
        
        // Propiedad pública de ámbito estático que utilizan los diferentes métodos.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static System.Management.ManagementScope StaticScope {
            get {
                return statMgmtScope;
            }
            set {
                statMgmtScope = value;
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAddressWidthNull {
            get {
                if ((curObj["AddressWidth"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Ancho de dirección de procesador en bits.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort AddressWidth {
            get {
                if ((curObj["AddressWidth"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["AddressWidth"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsArchitectureNull {
            get {
                if ((curObj["Architecture"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Architecture especifica la arquitectura del procesador usada por est" +
            "a plataforma. Devuelve uno de los siguientes valores enteros:\n0 - x86 \n1 - MIPS " +
            "\n2 - Alpha \n3 - PowerPC \n6 - ia64 \n9 - x64 \n")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ArchitectureValues Architecture {
            get {
                if ((curObj["Architecture"] == null)) {
                    return ((ArchitectureValues)(System.Convert.ToInt32(10)));
                }
                return ((ArchitectureValues)(System.Convert.ToInt32(curObj["Architecture"])));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAvailabilityNull {
            get {
                if ((curObj["Availability"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"La disponibilidad y estado del dispositivo. Por ejemplo, la propiedad disponibilidad, indica que el dispositivo está en funcionamiento y tiene energía total (valor=3), o se encuentra en un estado de aviso (4), prueba (5), degradado (10) o ahorro de energía (valores 13-15 y 17). En relación con los estados de ahorro de energía, éstos se definen como sigue: Valor 13 (""Ahorro de energía: desconocido"") indica que se sabe que el dispositivo está en un modo de ahorro de energía, pero se desconoce su estado exacto en este modo; 14 (""Ahorro de energía: modo de bajo consumo"") indica que el dispositivo está en un estado de  ahorro de energía, pero sigue funcionando y puede exhibir una baja de rendimiento;  15 (""Ahorro de energía: espera"") describe que el sistema no está en funcionamiento, pero que se podría poner en operación ""rápidamente""; y valor 17 (""Ahorro de energía: advertencia"") indica que el equipo está en un estado de aviso, aunque está también en modo de ahorro de energía.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public AvailabilityValues Availability {
            get {
                if ((curObj["Availability"] == null)) {
                    return ((AvailabilityValues)(System.Convert.ToInt32(0)));
                }
                return ((AvailabilityValues)(System.Convert.ToInt32(curObj["Availability"])));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Caption es una descripción textual breve (cadena de una línea) del o" +
            "bjeto.")]
        public string Caption {
            get {
                return ((string)(curObj["Caption"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsConfigManagerErrorCodeNull {
            get {
                if ((curObj["ConfigManagerErrorCode"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Indica el código de error del Administrador de configuración de Win32. Los valore" +
            "s siguientes pueden ser devueltos: \n0 Este dispositivo funciona correctamente. \n" +
            "1 Este dispositivo no está configurado correctamente. \n2 Windows no puede cargar" +
            " el controlador para este dispositivo. \n3 El controlador de este dispositivo pue" +
            "de estar dañado o le falta memoria o recursos a su sistema. \n4 Este dispositivo " +
            "no funciona correctamente. Uno de sus controladores o el Registro pueden estar d" +
            "añados. \n5 El controlador de este dispositivo necesita un recurso que Windows no" +
            " puede administrar. \n6 La configuración de arranque de este dispositivo entra en" +
            " conflicto con otros dispositivos. \n7 No se puede filtrar. \n8 Falta el cargador " +
            "de controlador del dispositivo. \n9 Este dispositivo no funciona correctamente po" +
            "rque el firmware de control está informando incorrectamente acerca de los recurs" +
            "os del dispositivo. \n10 El dispositivo no puede se iniciar. \n11 Error en el disp" +
            "ositivo. \n12 Este dispositivo no encuentra suficientes recursos libres para usar" +
            ". \n13 Windows no puede comprobar los recursos de este dispositivo. \n14 Este disp" +
            "ositivo no funcionará correctamente hasta que reinicie su equipo. \n15 Este dispo" +
            "sitivo no funciona correctamente porque hay un posible problema de enumeración. " +
            "\n16 Windows no puede identificar todos los recursos que utiliza este dispositivo" +
            ". \n17 Este dispositivo está solicitando un tipo de recurso desconocido. \n18 Vuel" +
            "va a instalar los controladores de este dispositivo \n19 Su Registro debe estar d" +
            "añado. \n20 Error usar el cargador VxD. \n21 Error del sistema: intente cambiar el" +
            " controlador de este dispositivo. Si esto no funciona, consulte la documentación" +
            " de hardware. Windows está quitando este dispositivo. \n22 Este dispositivo está " +
            "deshabilitado. \n23 Error del sistema: intente cambiar el controlador de este dis" +
            "positivo. Si esto no funciona, consulte la documentación de hardware. \n24 Este d" +
            "ispositivo no está presente, no funciona correctamente o no tiene todos los cont" +
            "roladores instalados. \n25 Windows aún está instalando este dispositivo. \n26 Wind" +
            "ows aún está instalando este dispositivo. \n27 Este dispositivo no tiene una conf" +
            "iguración de Registro válida. \n28 Los controladores de este dispositivo no están" +
            " instalados. \n29 Este dispositivo está deshabilitado porque el firmware no propo" +
            "rcionó los recursos requeridos. \n30 Este dispositivo está utilizando una recurso" +
            " de solicitud de interrupción (IRQ) que ya está usando otro dispositivo. \n31 Est" +
            "e dispositivo no funciona correctamente porque Windows no puede cargar los contr" +
            "oladores requeridos.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ConfigManagerErrorCodeValues ConfigManagerErrorCode {
            get {
                if ((curObj["ConfigManagerErrorCode"] == null)) {
                    return ((ConfigManagerErrorCodeValues)(System.Convert.ToInt32(32)));
                }
                return ((ConfigManagerErrorCodeValues)(System.Convert.ToInt32(curObj["ConfigManagerErrorCode"])));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsConfigManagerUserConfigNull {
            get {
                if ((curObj["ConfigManagerUserConfig"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Indica si el dispositivo usa una configuración predefinida por el usuario.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool ConfigManagerUserConfig {
            get {
                if ((curObj["ConfigManagerUserConfig"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["ConfigManagerUserConfig"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCpuStatusNull {
            get {
                if ((curObj["CpuStatus"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad CpuStatus especifica el estado actual del procesador. Los cambios de" +
            " estado surgen al usar el procesador, no dependen de su condición física.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public CpuStatusValues CpuStatus {
            get {
                if ((curObj["CpuStatus"] == null)) {
                    return ((CpuStatusValues)(System.Convert.ToInt32(8)));
                }
                return ((CpuStatusValues)(System.Convert.ToInt32(curObj["CpuStatus"])));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"CreationClassName indica el nombre de la clase o subclase que se usa en la creación de una instancia. Cuando se usa con las demás propiedades clave de esta clase, esta propiedad permite que se identifiquen de manera única todas las instancias de esta clase y sus subclases.")]
        public string CreationClassName {
            get {
                return ((string)(curObj["CreationClassName"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCurrentClockSpeedNull {
            get {
                if ((curObj["CurrentClockSpeed"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La velocidad actual (en MHz) de este procesador.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint CurrentClockSpeed {
            get {
                if ((curObj["CurrentClockSpeed"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["CurrentClockSpeed"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCurrentVoltageNull {
            get {
                if ((curObj["CurrentVoltage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"La propiedad CurrentVoltage especifica el voltaje del procesador. Los bits 0 a 6 del campo contienen el voltaje actual del procesador multiplicado por diez. Este valor sólo se establece cuando el SMBIOS designa un valor de voltaje. Para obtener valores específicos, consulte VoltageCaps.
Ejemplo: el valor del campo para un voltaje de procesador de 1,8 voltios sería 92h = 80h + (1,8 x 10) = 80h + 18 = 80h + 12h.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort CurrentVoltage {
            get {
                if ((curObj["CurrentVoltage"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["CurrentVoltage"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDataWidthNull {
            get {
                if ((curObj["DataWidth"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Ancho de datos de procesador en bits.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort DataWidth {
            get {
                if ((curObj["DataWidth"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["DataWidth"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Description ofrece una descripción textual del objeto. ")]
        public string Description {
            get {
                return ((string)(curObj["Description"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad DeviceID contiene una cadena que identifica de forma única el proces" +
            "ador con respecto a otros dispositivos del sistema.")]
        public string DeviceID {
            get {
                return ((string)(curObj["DeviceID"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsErrorClearedNull {
            get {
                if ((curObj["ErrorCleared"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("ErrorCleared es una propiedad booleana que indica que el error comunicado en la p" +
            "ropiedad LastErrorCode se ha resuelto ahora.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool ErrorCleared {
            get {
                if ((curObj["ErrorCleared"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["ErrorCleared"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("ErrorDescription es una cadena de forma libre que ofrece más información acerca d" +
            "el error registrado en la propiedad LastErrorCode e información acerca de cualqu" +
            "ier acción correctiva que se pueda tomar.")]
        public string ErrorDescription {
            get {
                return ((string)(curObj["ErrorDescription"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsExtClockNull {
            get {
                if ((curObj["ExtClock"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad ExtClock especifica la frecuencia del reloj externo. Si la frecuenci" +
            "a no se conoce, esta propiedad se establece como null.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ExtClock {
            get {
                if ((curObj["ExtClock"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["ExtClock"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFamilyNull {
            get {
                if ((curObj["Family"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("El tipo de familia del procesador. Por ejemplo, los valores incluyen \"Procesador " +
            "Pentium(R) con tecnología MMX(TM)\" (14) y \"68040\" (96).")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public FamilyValues Family {
            get {
                if ((curObj["Family"] == null)) {
                    return ((FamilyValues)(System.Convert.ToInt32(0)));
                }
                return ((FamilyValues)(System.Convert.ToInt32(curObj["Family"])));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInstallDateNull {
            get {
                if ((curObj["InstallDate"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad InstallDate es un valor de fecha y hora que indica cuándo se instaló" +
            " el objeto. La falta de un valor no indica que el objeto no está instalado.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public System.DateTime InstallDate {
            get {
                if ((curObj["InstallDate"] != null)) {
                    return ToDateTime(((string)(curObj["InstallDate"])));
                }
                else {
                    return System.DateTime.MinValue;
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsL2CacheSizeNull {
            get {
                if ((curObj["L2CacheSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad L2CacheSize especifica el tamaño de la caché de nivel 2 del procesad" +
            "or. Una caché de nivel 2 es un área de memoria externa que presenta un tiempo de" +
            " acceso más rápido que la memoria RAM principal.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint L2CacheSize {
            get {
                if ((curObj["L2CacheSize"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["L2CacheSize"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsL2CacheSpeedNull {
            get {
                if ((curObj["L2CacheSpeed"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad L2CacheSpeed especifica la velocidad de reloj de la caché de nivel 2" +
            " del procesador. Una caché de nivel 2 es un área de memoria externa que presenta" +
            " un tiempo de acceso más rápido que la memoria RAM principal.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint L2CacheSpeed {
            get {
                if ((curObj["L2CacheSpeed"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["L2CacheSpeed"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsL3CacheSizeNull {
            get {
                if ((curObj["L3CacheSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad L3CacheSize especifica el tamaño de la caché de nivel 3 del procesad" +
            "or. Una caché de nivel 3 es un área de memoria externa que presenta un tiempo de" +
            " acceso más rápido que la memoria RAM principal.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint L3CacheSize {
            get {
                if ((curObj["L3CacheSize"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["L3CacheSize"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsL3CacheSpeedNull {
            get {
                if ((curObj["L3CacheSpeed"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad L3CacheSpeed especifica la velocidad de reloj de la caché de nivel 3" +
            " del procesador. Una caché de nivel 3 es un área de memoria externa que presenta" +
            " un tiempo de acceso más rápido que la memoria RAM principal.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint L3CacheSpeed {
            get {
                if ((curObj["L3CacheSpeed"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["L3CacheSpeed"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLastErrorCodeNull {
            get {
                if ((curObj["LastErrorCode"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("LastErrorCode captura el último código de error informado por el dispositivo lógi" +
            "co.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint LastErrorCode {
            get {
                if ((curObj["LastErrorCode"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["LastErrorCode"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLevelNull {
            get {
                if ((curObj["Level"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Level define con más detalle el tipo de procesador. El valor depende" +
            " de la arquitectura del procesador.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort Level {
            get {
                if ((curObj["Level"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["Level"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLoadPercentageNull {
            get {
                if ((curObj["LoadPercentage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad LoadPercentage especifica la capacidad de carga de cada procesador c" +
            "omo promedio en el último segundo. El término \'carga del procesador\' se refiere " +
            "a la carga total que cada procesador detenta en cada momento.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort LoadPercentage {
            get {
                if ((curObj["LoadPercentage"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["LoadPercentage"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Manufacturer indica el fabricante del procesador.\nEjemplo: GenuineSi" +
            "licon")]
        public string Manufacturer {
            get {
                return ((string)(curObj["Manufacturer"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMaxClockSpeedNull {
            get {
                if ((curObj["MaxClockSpeed"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La velocidad máxima (en MHz) de este procesador.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MaxClockSpeed {
            get {
                if ((curObj["MaxClockSpeed"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["MaxClockSpeed"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Name define la etiqueta por la que se conoce el objeto. Cuando se in" +
            "cluye en una subclase, la propiedad Name puede ser invalidada como si se tratara" +
            " de una propiedad Key.")]
        public string Name {
            get {
                return ((string)(curObj["Name"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumberOfCoresNull {
            get {
                if ((curObj["NumberOfCores"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad NumberOfCores contiene el número total de núcleos de un procesador. " +
            "Por ejemplo, un equipo con un procesador dual tiene NumberOfCores = 2.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint NumberOfCores {
            get {
                if ((curObj["NumberOfCores"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["NumberOfCores"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumberOfLogicalProcessorsNull {
            get {
                if ((curObj["NumberOfLogicalProcessors"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad NumberOfLogicalProcessors especifica el número total de procesadores" +
            " lógicos.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint NumberOfLogicalProcessors {
            get {
                if ((curObj["NumberOfLogicalProcessors"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["NumberOfLogicalProcessors"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Una cadena que describe el tipo de familia del procesador. Utilizado cuando la pr" +
            "opiedad familia esté establecida como 1 (\"Otros\"). Esta cadena debe ser establec" +
            "ida como NULL cuando la propiedad de la familia es cualquier valor diferente de " +
            "1.")]
        public string OtherFamilyDescription {
            get {
                return ((string)(curObj["OtherFamilyDescription"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Indica el id. Plug and Play Win32 del dispositivo lógico. Ejemplo: *PNP030b")]
        public string PNPDeviceID {
            get {
                return ((string)(curObj["PNPDeviceID"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Indica los recursos específicos relacionados con energía de dispositivo lógico. Los valores de la matriz, 0=""Desconocido"", 1=""No compatible"" y 2=""Deshabilitado"" se explican por sí solos. El valor 3=""Habilitado"" indica que las características de administración de energía están habilitadas actualmente pero se desconoce el conjunto de características exacto o la información no está disponible. "" Modos de ahorro de energía establecidos automáticamente "" (4) describe que un dispositivo puede cambiar su estado de energía con base en el uso u otros criterios. "" Estado de energía configurable "" (5) indica que se admite el método SetPowerState. "" Ciclo de energía permitido "" (6) indica que se puede invocar el método SetPowerState con la variable de entrada PowerState establecida a 5 (""Ciclo de energía ""). "" Se admite el encendido por tiempo "" (7) indica que el método SetPowerState puede ser invocado con la variable de entrada PowerState establecida  a 5 (""Ciclo de energía "") y el parámetro Time establecido a un fecha y hora específica, o intervalo, para encendido.")]
        public PowerManagementCapabilitiesValues[] PowerManagementCapabilities {
            get {
                System.Array arrEnumVals = ((System.Array)(curObj["PowerManagementCapabilities"]));
                PowerManagementCapabilitiesValues[] enumToRet = new PowerManagementCapabilitiesValues[arrEnumVals.Length];
                int counter = 0;
                for (counter = 0; (counter < arrEnumVals.Length); counter = (counter + 1)) {
                    enumToRet[counter] = ((PowerManagementCapabilitiesValues)(System.Convert.ToInt32(arrEnumVals.GetValue(counter))));
                }
                return enumToRet;
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPowerManagementSupportedNull {
            get {
                if ((curObj["PowerManagementSupported"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Booleano que indica que el Dispositivo se puede administrar con energía - por ej., ponerlo en un estado de ahorro de energía. Este booleano no indica que las características de administración de energía están actualmente habilitadas, o si están deshabilitadas, las características que son compatibles. Consulte la matriz PowerManagementCapabilities para obtener esta información. Si este booleano es falso, el valor entero 1, para la cadena, ""No compatible"", debe ser la única entrada en la matriz PowerManagementCapabilities.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool PowerManagementSupported {
            get {
                if ((curObj["PowerManagementSupported"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["PowerManagementSupported"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"La propiedad ProcessorId contiene información específica del procesador que describe sus características. Para las CPU de clase x86, el formato del campo depende de si el procesador admite la instrucción CPUID. Si es así, la propiedad ProcessorId contiene dos valores con formato DWORD. El primero (entre las direcciones 08h y 0Bh) es el valor del registro EAX devuelto por una instrucción CPUID con el EAX de entrada establecido como 1. El segundo (entre las direcciones 0Ch y 0Fh) es el valor del registro EDX devuelto por esa instrucción. Sólo son significativos los dos primeros bytes de la propiedad ProcessorID (los demás se establecen como cero) y almacenan, en formato WORD, el contenido del registro DX al restablecer la CPU.")]
        public string ProcessorId {
            get {
                return ((string)(curObj["ProcessorId"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsProcessorTypeNull {
            get {
                if ((curObj["ProcessorType"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad ProcessorType especifica la función principal del procesador.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ProcessorTypeValues ProcessorType {
            get {
                if ((curObj["ProcessorType"] == null)) {
                    return ((ProcessorTypeValues)(System.Convert.ToInt32(0)));
                }
                return ((ProcessorTypeValues)(System.Convert.ToInt32(curObj["ProcessorType"])));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRevisionNull {
            get {
                if ((curObj["Revision"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Revision especifica el nivel de revisión dependiente de la arquitect" +
            "ura del sistema. El significado de este valor depende de la arquitectura del pro" +
            "cesador. Contiene los mismos valores que el miembro \"Versión\", pero en formato n" +
            "umérico.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort Revision {
            get {
                if ((curObj["Revision"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["Revision"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Una cadena de forma libre que describe el rol del procesador  - por ejemplo,  \"Pr" +
            "ocesador central\"\' o \"Procesador matemático\"")]
        public string Role {
            get {
                return ((string)(curObj["Role"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSecondLevelAddressTranslationExtensionsNull {
            get {
                if ((curObj["SecondLevelAddressTranslationExtensions"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad SecondLevelAddressTranslationExtensions determina si el procesador a" +
            "dmite el uso de extensiones de traducción de direcciones para virtualización.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool SecondLevelAddressTranslationExtensions {
            get {
                if ((curObj["SecondLevelAddressTranslationExtensions"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["SecondLevelAddressTranslationExtensions"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad SocketDesignation contiene el tipo de zócalo de chip usado en el cir" +
            "cuito.\nEjemplo: J202")]
        public string SocketDesignation {
            get {
                return ((string)(curObj["SocketDesignation"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"La propiedad Status es una cadena que indica el estado actual del objeto. Se pueden definir diversos estados operativos y no operativos. Los estados operativos son ""Correcto"", ""Degradado"" y ""Pred. de error"". ""Pred. de error"" indica que quizá un elemento funcione correctamente pero que anticipe un error en el futuro cercano. Un ejemplo es un disco duro compatible con SMART. También se pueden especificar estados no operativos. Éstos son ""Error"", ""Iniciando"", ""Deteniendo"" y ""Servicio"". El último, ""Servicio"", se podría aplicar durante la creación del reflejo de un disco, la recarga de una lista de permisos de usuarios u otro trabajo administrativo. No todo este trabajo se realiza en línea y no obstante el elemento administrado no es ""Correcto"" ni se encuentra en uno de los otros estados.")]
        public string Status {
            get {
                return ((string)(curObj["Status"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsStatusInfoNull {
            get {
                if ((curObj["StatusInfo"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"StatusInfo es una cadena que indica si el dispositivo lógico está en un estado habilitado (valor = 3), deshabilitado (valor = 4) o algún otro estado (1) o un estado desconocido (2). Si esta propiedad no se aplica al dispositivo lógico, el valor, 5 (""No aplicable""), debe ser usado.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public StatusInfoValues StatusInfo {
            get {
                if ((curObj["StatusInfo"] == null)) {
                    return ((StatusInfoValues)(System.Convert.ToInt32(0)));
                }
                return ((StatusInfoValues)(System.Convert.ToInt32(curObj["StatusInfo"])));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("El submodelo es una cadena con formato libre que indica el nivel de revisión del " +
            "procesador dentro de la familia de procesadores.")]
        public string Stepping {
            get {
                return ((string)(curObj["Stepping"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("CreationClassName de ámbito del sistema.")]
        public string SystemCreationClassName {
            get {
                return ((string)(curObj["SystemCreationClassName"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Nombre del sistema de ámbito.")]
        public string SystemName {
            get {
                return ((string)(curObj["SystemName"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Un identificador global único para el procesador. Este identificador sólo puede s" +
            "er único dentro de una familia de procesadores.")]
        public string UniqueId {
            get {
                return ((string)(curObj["UniqueId"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsUpgradeMethodNull {
            get {
                if ((curObj["UpgradeMethod"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Información del socket de la CPU incluyendo datos acerca de como se puede actuali" +
            "zar este procesador (si las actualizaciones son compaltibles). Esta propiedad es" +
            " una enumeración de enteros.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public UpgradeMethodValues UpgradeMethod {
            get {
                if ((curObj["UpgradeMethod"] == null)) {
                    return ((UpgradeMethodValues)(System.Convert.ToInt32(0)));
                }
                return ((UpgradeMethodValues)(System.Convert.ToInt32(curObj["UpgradeMethod"])));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad Version especifica un número de revisión de procesador dependiente d" +
            "e la arquitectura. Nota: este miembro no se usa en Windows 95.\nEjemplo: Modelo 2" +
            ", submodelo 12.")]
        public string Version {
            get {
                return ((string)(curObj["Version"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVirtualizationFirmwareEnabledNull {
            get {
                if ((curObj["VirtualizationFirmwareEnabled"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad VirtualizationFirmwareEnabled determina si el firmware tiene habilit" +
            "adas extensiones de virtualización.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool VirtualizationFirmwareEnabled {
            get {
                if ((curObj["VirtualizationFirmwareEnabled"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["VirtualizationFirmwareEnabled"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVMMonitorModeExtensionsNull {
            get {
                if ((curObj["VMMonitorModeExtensions"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("La propiedad VMMonitorModeExtensions determina si el procesador admite extensione" +
            "s de Monitor de equipo virtual Intel o AMD.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool VMMonitorModeExtensions {
            get {
                if ((curObj["VMMonitorModeExtensions"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["VMMonitorModeExtensions"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVoltageCapsNull {
            get {
                if ((curObj["VoltageCaps"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"La propiedad VoltageCaps especifica las capacidades de voltaje del procesador. Los bits 0 a 3 del campo representan voltajes específicos que acepta el zócalo del procesador. El resto de los bits se deben establecer como cero. Es posible configurar el zócalo mediante la definición de varios bits. Para obtener un intervalo de voltajes, consulte CurrentVoltage. Si la propiedad es NULL, las capacidades de voltaje se desconocen.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public VoltageCapsValues VoltageCaps {
            get {
                if ((curObj["VoltageCaps"] == null)) {
                    return ((VoltageCapsValues)(System.Convert.ToInt32(8)));
                }
                return ((VoltageCapsValues)(System.Convert.ToInt32(curObj["VoltageCaps"])));
            }
        }
        
        private bool CheckIfProperClass(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions OptionsParam) {
            if (((path != null) 
                        && (string.Compare(path.ClassName, this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                return CheckIfProperClass(new System.Management.ManagementObject(mgmtScope, path, OptionsParam));
            }
        }
        
        private bool CheckIfProperClass(System.Management.ManagementBaseObject theObj) {
            if (((theObj != null) 
                        && (string.Compare(((string)(theObj["__CLASS"])), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                System.Array parentClasses = ((System.Array)(theObj["__DERIVATION"]));
                if ((parentClasses != null)) {
                    int count = 0;
                    for (count = 0; (count < parentClasses.Length); count = (count + 1)) {
                        if ((string.Compare(((string)(parentClasses.GetValue(count))), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        private bool ShouldSerializeAddressWidth() {
            if ((this.IsAddressWidthNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeArchitecture() {
            if ((this.IsArchitectureNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeAvailability() {
            if ((this.IsAvailabilityNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeConfigManagerErrorCode() {
            if ((this.IsConfigManagerErrorCodeNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeConfigManagerUserConfig() {
            if ((this.IsConfigManagerUserConfigNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeCpuStatus() {
            if ((this.IsCpuStatusNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeCurrentClockSpeed() {
            if ((this.IsCurrentClockSpeedNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeCurrentVoltage() {
            if ((this.IsCurrentVoltageNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeDataWidth() {
            if ((this.IsDataWidthNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeErrorCleared() {
            if ((this.IsErrorClearedNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeExtClock() {
            if ((this.IsExtClockNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeFamily() {
            if ((this.IsFamilyNull == false)) {
                return true;
            }
            return false;
        }
        
        // Convierte una fecha y hora determinadas con formato DMTF en un objeto System.DateTime.
        static System.DateTime ToDateTime(string dmtfDate) {
            System.DateTime initializer = System.DateTime.MinValue;
            int year = initializer.Year;
            int month = initializer.Month;
            int day = initializer.Day;
            int hour = initializer.Hour;
            int minute = initializer.Minute;
            int second = initializer.Second;
            long ticks = 0;
            string dmtf = dmtfDate;
            System.DateTime datetime = System.DateTime.MinValue;
            string tempString = string.Empty;
            if ((dmtf == null)) {
                throw new System.ArgumentOutOfRangeException();
            }
            if ((dmtf.Length == 0)) {
                throw new System.ArgumentOutOfRangeException();
            }
            if ((dmtf.Length != 25)) {
                throw new System.ArgumentOutOfRangeException();
            }
            try {
                tempString = dmtf.Substring(0, 4);
                if (("****" != tempString)) {
                    year = int.Parse(tempString);
                }
                tempString = dmtf.Substring(4, 2);
                if (("**" != tempString)) {
                    month = int.Parse(tempString);
                }
                tempString = dmtf.Substring(6, 2);
                if (("**" != tempString)) {
                    day = int.Parse(tempString);
                }
                tempString = dmtf.Substring(8, 2);
                if (("**" != tempString)) {
                    hour = int.Parse(tempString);
                }
                tempString = dmtf.Substring(10, 2);
                if (("**" != tempString)) {
                    minute = int.Parse(tempString);
                }
                tempString = dmtf.Substring(12, 2);
                if (("**" != tempString)) {
                    second = int.Parse(tempString);
                }
                tempString = dmtf.Substring(15, 6);
                if (("******" != tempString)) {
                    ticks = (long.Parse(tempString) * ((long)((System.TimeSpan.TicksPerMillisecond / 1000))));
                }
                if (((((((((year < 0) 
                            || (month < 0)) 
                            || (day < 0)) 
                            || (hour < 0)) 
                            || (minute < 0)) 
                            || (minute < 0)) 
                            || (second < 0)) 
                            || (ticks < 0))) {
                    throw new System.ArgumentOutOfRangeException();
                }
            }
            catch (System.Exception e) {
                throw new System.ArgumentOutOfRangeException(null, e.Message);
            }
            datetime = new System.DateTime(year, month, day, hour, minute, second, 0);
            datetime = datetime.AddTicks(ticks);
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
            int UTCOffset = 0;
            int OffsetToBeAdjusted = 0;
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            tempString = dmtf.Substring(22, 3);
            if ((tempString != "******")) {
                tempString = dmtf.Substring(21, 4);
                try {
                    UTCOffset = int.Parse(tempString);
                }
                catch (System.Exception e) {
                    throw new System.ArgumentOutOfRangeException(null, e.Message);
                }
                OffsetToBeAdjusted = ((int)((OffsetMins - UTCOffset)));
                datetime = datetime.AddMinutes(((double)(OffsetToBeAdjusted)));
            }
            return datetime;
        }
        
        // Convierte un objeto System.DateTime determinado al formato de fecha y hora DMTF.
        static string ToDmtfDateTime(System.DateTime date) {
            string utcString = string.Empty;
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(date);
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            if ((System.Math.Abs(OffsetMins) > 999)) {
                date = date.ToUniversalTime();
                utcString = "+000";
            }
            else {
                if ((tickOffset.Ticks >= 0)) {
                    utcString = string.Concat("+", ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute))).ToString().PadLeft(3, '0'));
                }
                else {
                    string strTemp = ((long)(OffsetMins)).ToString();
                    utcString = string.Concat("-", strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
                }
            }
            string dmtfDateTime = ((int)(date.Year)).ToString().PadLeft(4, '0');
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Month)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Day)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Hour)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Minute)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Second)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ".");
            System.DateTime dtTemp = new System.DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
            long microsec = ((long)((((date.Ticks - dtTemp.Ticks) 
                        * 1000) 
                        / System.TimeSpan.TicksPerMillisecond)));
            string strMicrosec = ((long)(microsec)).ToString();
            if ((strMicrosec.Length > 6)) {
                strMicrosec = strMicrosec.Substring(0, 6);
            }
            dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, utcString);
            return dmtfDateTime;
        }
        
        private bool ShouldSerializeInstallDate() {
            if ((this.IsInstallDateNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeL2CacheSize() {
            if ((this.IsL2CacheSizeNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeL2CacheSpeed() {
            if ((this.IsL2CacheSpeedNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeL3CacheSize() {
            if ((this.IsL3CacheSizeNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeL3CacheSpeed() {
            if ((this.IsL3CacheSpeedNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeLastErrorCode() {
            if ((this.IsLastErrorCodeNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeLevel() {
            if ((this.IsLevelNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeLoadPercentage() {
            if ((this.IsLoadPercentageNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeMaxClockSpeed() {
            if ((this.IsMaxClockSpeedNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeNumberOfCores() {
            if ((this.IsNumberOfCoresNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeNumberOfLogicalProcessors() {
            if ((this.IsNumberOfLogicalProcessorsNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializePowerManagementSupported() {
            if ((this.IsPowerManagementSupportedNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeProcessorType() {
            if ((this.IsProcessorTypeNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeRevision() {
            if ((this.IsRevisionNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeSecondLevelAddressTranslationExtensions() {
            if ((this.IsSecondLevelAddressTranslationExtensionsNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeStatusInfo() {
            if ((this.IsStatusInfoNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeUpgradeMethod() {
            if ((this.IsUpgradeMethodNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeVirtualizationFirmwareEnabled() {
            if ((this.IsVirtualizationFirmwareEnabledNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeVMMonitorModeExtensions() {
            if ((this.IsVMMonitorModeExtensionsNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeVoltageCaps() {
            if ((this.IsVoltageCapsNull == false)) {
                return true;
            }
            return false;
        }
        
        [Browsable(true)]
        public void CommitObject() {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put();
            }
        }
        
        [Browsable(true)]
        public void CommitObject(System.Management.PutOptions putOptions) {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put(putOptions);
            }
        }
        
        private void Initialize() {
            AutoCommitProp = true;
            isEmbedded = false;
        }
        
        private static string ConstructPath(string keyDeviceID) {
            string strPath = "ROOT\\CIMV2:Win32_Processor";
            strPath = string.Concat(strPath, string.Concat(".DeviceID=", string.Concat("\"", string.Concat(keyDeviceID, "\""))));
            return strPath;
        }
        
        private void InitializeObject(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            Initialize();
            if ((path != null)) {
                if ((CheckIfProperClass(mgmtScope, path, getOptions) != true)) {
                    throw new System.ArgumentException("El nombre de clase no coincide.");
                }
            }
            PrivateLateBoundObject = new System.Management.ManagementObject(mgmtScope, path, getOptions);
            PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
            curObj = PrivateLateBoundObject;
        }
        
        // Diferentes sobrecargas de ayuda GetInstances() para enumerar instancias de la clase WMI.
        public static ProcessorCollection GetInstances() {
            return GetInstances(null, null, null);
        }
        
        public static ProcessorCollection GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }
        
        public static ProcessorCollection GetInstances(string[] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }
        
        public static ProcessorCollection GetInstances(string condition, string[] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }
        
        public static ProcessorCollection GetInstances(System.Management.ManagementScope mgmtScope, System.Management.EnumerationOptions enumOptions) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\CIMV2";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementPath pathObj = new System.Management.ManagementPath();
            pathObj.ClassName = "Win32_Processor";
            pathObj.NamespacePath = "root\\CIMV2";
            System.Management.ManagementClass clsObject = new System.Management.ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null)) {
                enumOptions = new System.Management.EnumerationOptions();
                enumOptions.EnsureLocatable = true;
            }
            return new ProcessorCollection(clsObject.GetInstances(enumOptions));
        }
        
        public static ProcessorCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }
        
        public static ProcessorCollection GetInstances(System.Management.ManagementScope mgmtScope, string[] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }
        
        public static ProcessorCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition, string[] selectedProperties) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\CIMV2";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementObjectSearcher ObjectSearcher = new System.Management.ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_Processor", condition, selectedProperties));
            System.Management.EnumerationOptions enumOptions = new System.Management.EnumerationOptions();
            enumOptions.EnsureLocatable = true;
            ObjectSearcher.Options = enumOptions;
            return new ProcessorCollection(ObjectSearcher.Get());
        }
        
        [Browsable(true)]
        public static Processor CreateInstance() {
            System.Management.ManagementScope mgmtScope = null;
            if ((statMgmtScope == null)) {
                mgmtScope = new System.Management.ManagementScope();
                mgmtScope.Path.NamespacePath = CreatedWmiNamespace;
            }
            else {
                mgmtScope = statMgmtScope;
            }
            System.Management.ManagementPath mgmtPath = new System.Management.ManagementPath(CreatedClassName);
            System.Management.ManagementClass tmpMgmtClass = new System.Management.ManagementClass(mgmtScope, mgmtPath, null);
            return new Processor(tmpMgmtClass.CreateInstance());
        }
        
        [Browsable(true)]
        public void Delete() {
            PrivateLateBoundObject.Delete();
        }
        
        public uint Reset() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Reset", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint SetPowerState(ushort PowerState, System.DateTime Time) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("SetPowerState");
                inParams["PowerState"] = ((ushort)(PowerState));
                inParams["Time"] = ToDmtfDateTime(((System.DateTime)(Time)));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("SetPowerState", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public enum ArchitectureValues {
            
            X86 = 0,
            
            MIPS = 1,
            
            Opacidad_Alpha_ = 2,
            
            PowerPC = 3,
            
            Ia64 = 6,
            
            X64 = 9,
            
            NULL_ENUM_VALUE = 10,
        }
        
        public enum AvailabilityValues {
            
            Otros = 1,
            
            Desconocido = 2,
            
            Funcionar_Energía_completa = 3,
            
            Advertencia = 4,
            
            En_prueba = 5,
            
            No_aplicable = 6,
            
            Apagado = 7,
            
            Sin_conexión_a_la_red = 8,
            
            Inactivo = 9,
            
            Degradado = 10,
            
            No_instalado = 11,
            
            Error_de_instalación = 12,
            
            Ahorro_de_energía_desconocido = 13,
            
            Ahorro_de_energía_modo_de_bajo_consumo = 14,
            
            Ahorro_de_energía_espera = 15,
            
            Ciclo_de_energía = 16,
            
            Ahorro_de_energía_advertencia = 17,
            
            Pausado = 18,
            
            No_está_listo = 19,
            
            No_configurado = 20,
            
            Inactivo0 = 21,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum ConfigManagerErrorCodeValues {
            
            Este_dispositivo_funciona_correctamente_ = 0,
            
            El_dispositivo_no_está_configurado_correctamente_ = 1,
            
            Windows_no_puede_cargar_el_controlador_para_este_dispositivo_ = 2,
            
            El_controlador_de_este_dispositivo_podría_estar_dañado_o_es_posible_que_su_sistema_tenga_poca_memoria_u_otros_recursos_ = 3,
            
            Este_dispositivo_no_funciona_correctamente_Podría_estar_dañado_uno_de_sus_controladores_o_el_Registro_ = 4,
            
            El_controlador_de_este_dispositivo_necesita_un_recurso_que_Windows_no_puede_administrar_ = 5,
            
            La_configuración_de_arranque_de_este_dispositivo_está_en_conflicto_con_otros_dispositivos_ = 6,
            
            No_se_puede_filtrar_ = 7,
            
            Falta_el_controlador_del_dispositivo_ = 8,
            
            Este_dispositivo_no_funciona_correctamente_porque_el_firmware_de_control_informa_incorrectamente_de_los_recursos_del_dispositivo_ = 9,
            
            No_puede_iniciar_este_dispositivo_ = 10,
            
            Error_de_este_dispositivo_ = 11,
            
            Este_dispositivo_no_encuentra_suficientes_recursos_libres_que_pueda_usar_ = 12,
            
            Windows_no_puede_comprobar_los_recursos_de_este_dispositivo_ = 13,
            
            El_dispositivo_no_puede_funcionar_correctamente_hasta_que_reinicie_su_equipo_ = 14,
            
            Este_dispositivo_no_funciona_correctamente_porque_quizá_existe_un_problema_de_reenumeración_ = 15,
            
            Windows_no_puede_identificar_todos_los_recursos_que_usa_este_dispositivo_ = 16,
            
            Este_dispositivo_está_solicitando_un_tipo_de_recurso_desconocido_ = 17,
            
            Reinstalar_los_controladores_de_este_dispositivo_ = 18,
            
            Error_al_usar_el_cargador_VxD_ = 19,
            
            Su_Registro_podría_estar_dañado_ = 20,
            
            Error_del_sistema_pruebe_a_cambiar_el_controlador_de_este_dispositivo_Si_eso_no_funciona_consulte_la_documentación_del_hardware_Windows_quitará_este_dispositivo_ = 21,
            
            Este_dispositivo_está_deshabilitado_ = 22,
            
            Error_del_sistema_pruebe_a_cambiar_el_controlador_de_este_dispositivo_Si_eso_no_funciona_consulte_la_documentación_del_hardware_ = 23,
            
            Este_dispositivo_no_está_presente_no_funciona_correctamente_o_no_tiene_todos_sus_controladores_instalados_ = 24,
            
            Windows_sigue_configurando_este_dispositivo_ = 25,
            
            Windows_sigue_configurando_este_dispositivo_0 = 26,
            
            Este_dispositivo_no_tiene_una_configuración_de_registro_válida_ = 27,
            
            Los_controladores_de_este_dispositivo_no_están_instalados_ = 28,
            
            Este_dispositivo_está_deshabilitado_porque_su_firmware_no_le_proporcionó_los_recursos_requeridos_ = 29,
            
            Este_dispositivo_usa_un_recurso_de_solicitud_de_interrupción_IRQ_que_usa_otro_dispositivo_ = 30,
            
            Este_dispositivo_no_funciona_correctamente_porque_Windows_no_puede_cargar_los_controladores_requeridos_para_este_dispositivo_ = 31,
            
            NULL_ENUM_VALUE = 32,
        }
        
        public enum CpuStatusValues {
            
            Desconocido = 0,
            
            CPU_habilitada = 1,
            
            CPU_deshabilitada_por_el_usuario_a_través_de_la_configuración_del_BIOS = 2,
            
            CPU_deshabilitada_por_el_BIOS_error_POST_ = 3,
            
            CPU_inactiva = 4,
            
            Reservado = 5,
            
            Reservado0 = 6,
            
            Otros = 7,
            
            NULL_ENUM_VALUE = 8,
        }
        
        public enum FamilyValues {
            
            Otros = 1,
            
            Desconocido = 2,
            
            Val_8086 = 3,
            
            Val_80286 = 4,
            
            Val_80386 = 5,
            
            Val_80486 = 6,
            
            Val_8087 = 7,
            
            Val_80287 = 8,
            
            Val_80387 = 9,
            
            Val_80487 = 10,
            
            Marca_Pentium_R_ = 11,
            
            Pentium_R_Pro = 12,
            
            Pentium_R_II = 13,
            
            Procesador_Pentium_R_con_tecnología_MMX_TM_ = 14,
            
            Celeron_TM_ = 15,
            
            Pentium_R_II_Xeon_TM_ = 16,
            
            Pentium_R_III = 17,
            
            Familia_M1 = 18,
            
            Familia_M2 = 19,
            
            Familia_K5 = 24,
            
            Familia_K6 = 25,
            
            K6_2 = 26,
            
            K6_3 = 27,
            
            Familia_de_procesadores_AMD_Athlon_TM_ = 28,
            
            Procesador_AMD_R_Duron_TM_ = 29,
            
            AMD29000_Family = 30,
            
            K6_2_ = 31,
            
            Familia_Power_PC = 32,
            
            Power_PC_601 = 33,
            
            Power_PC_603 = 34,
            
            Power_PC_603_ = 35,
            
            Power_PC_604 = 36,
            
            Power_PC_620 = 37,
            
            Power_PC_X704 = 38,
            
            Power_PC_750 = 39,
            
            Familia_Alpha = 48,
            
            Alpha_21064 = 49,
            
            Alpha_21066 = 50,
            
            Alpha_21164 = 51,
            
            Alpha_21164PC = 52,
            
            Alpha_21164a = 53,
            
            Alpha_21264 = 54,
            
            Alpha_21364 = 55,
            
            Familia_MIPS = 64,
            
            MIPS_R4000 = 65,
            
            MIPS_R4200 = 66,
            
            MIPS_R4400 = 67,
            
            MIPS_R4600 = 68,
            
            MIPS_R10000 = 69,
            
            Familia_SPARC = 80,
            
            SuperSPARC = 81,
            
            MicroSPARC_II = 82,
            
            MicroSPARC_IIep = 83,
            
            UltraSPARC = 84,
            
            UltraSPARC_II = 85,
            
            UltraSPARC_IIi = 86,
            
            UltraSPARC_III0 = 87,
            
            UltraSPARC_IIIi = 88,
            
            Val_68040 = 96,
            
            Familia_68xxx = 97,
            
            Val_68000 = 98,
            
            Val_68010 = 99,
            
            Val_68020 = 100,
            
            Val_68030 = 101,
            
            Familia_Hobbit = 112,
            
            Familia_Crusoe_TM_TM5000 = 120,
            
            Familia_Crusoe_TM_TM3000 = 121,
            
            Familia_Efficeon_TM_TM8000 = 122,
            
            Weitek = 128,
            
            Procesador_Itanium_TM_ = 130,
            
            Familia_de_procesadores_AMD_Athlon_TM_64 = 131,
            
            Familia_AMD_Opteron_TM_ = 132,
            
            Familia_PA_RISC = 144,
            
            PA_RISC_8500 = 145,
            
            PA_RISC_8000 = 146,
            
            PA_RISC_7300LC = 147,
            
            PA_RISC_7200 = 148,
            
            PA_RISC_7100LC = 149,
            
            PA_RISC_7100 = 150,
            
            Familia_V30 = 160,
            
            Pentium_R_III_Xeon_TM_ = 176,
            
            Procesador_Pentium_R_III_con_tecnología_Intel_R_SpeedStep_TM_ = 177,
            
            Pentium_R_4 = 178,
            
            Intel_R_Xeon_TM_ = 179,
            
            Familia_AS400 = 180,
            
            Procesador_MP_Intel_R_Xeon_TM_ = 181,
            
            Familia_AMD_AthlonXP_TM_ = 182,
            
            Familia_AMD_AthlonMP_TM_ = 183,
            
            Intel_R_Itanium_R_2 = 184,
            
            Procesador_Intel_Pentium_M = 185,
            
            K7 = 190,
            
            Familia_IBM390 = 200,
            
            G4 = 201,
            
            G5 = 202,
            
            G6 = 203,
            
            Base_z_Architecture = 204,
            
            I860 = 250,
            
            I960 = 251,
            
            SH_3 = 260,
            
            SH_4 = 261,
            
            ARM = 280,
            
            StrongARM = 281,
            
            Val_6x86 = 300,
            
            MediaGX = 301,
            
            MII = 302,
            
            WinChip = 320,
            
            DSP = 350,
            
            Procesador_de_vídeo = 500,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum PowerManagementCapabilitiesValues {
            
            Desconocido = 0,
            
            No_compatible = 1,
            
            Deshabilitado = 2,
            
            Habilitado = 3,
            
            Modos_de_ahorro_de_energía_establecidos_automáticamente = 4,
            
            Estado_de_energía_configurable = 5,
            
            Ciclo_de_energía_permitido = 6,
            
            Se_admite_el_encendido_por_tiempo = 7,
            
            NULL_ENUM_VALUE = 8,
        }
        
        public enum ProcessorTypeValues {
            
            Otros = 1,
            
            Desconocido = 2,
            
            Procesador_central = 3,
            
            Procesador_matemático = 4,
            
            Procesador_DSP = 5,
            
            Procesador_de_vídeo = 6,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum StatusInfoValues {
            
            Otros = 1,
            
            Desconocido = 2,
            
            Habilitado = 3,
            
            Deshabilitado = 4,
            
            No_aplicable = 5,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum UpgradeMethodValues {
            
            Otros = 1,
            
            Desconocido = 2,
            
            Placa_base_secundaria = 3,
            
            Socket_ZIF = 4,
            
            Replacement_Piggy_Back = 5,
            
            Ninguna = 6,
            
            Socket_LIF = 7,
            
            Ranura_1 = 8,
            
            Ranura_2 = 9,
            
            Val_370_Pin_Socket = 10,
            
            Ranura_A = 11,
            
            Ranura_M = 12,
            
            Socket_423 = 13,
            
            Socket_A_Socket_462_ = 14,
            
            Socket_478 = 15,
            
            Socket_754 = 16,
            
            Socket_940 = 17,
            
            Socket_939 = 18,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum VoltageCapsValues {
            
            Val_5 = 1,
            
            Val_3_3 = 2,
            
            Val_2_9 = 4,
            
            NULL_ENUM_VALUE = 8,
        }
        
        // Implementación del enumerador para enumerar instancias de la clase.
        public class ProcessorCollection : object, ICollection {
            
            private ManagementObjectCollection privColObj;
            
            public ProcessorCollection(ManagementObjectCollection objCollection) {
                privColObj = objCollection;
            }
            
            public virtual int Count {
                get {
                    return privColObj.Count;
                }
            }
            
            public virtual bool IsSynchronized {
                get {
                    return privColObj.IsSynchronized;
                }
            }
            
            public virtual object SyncRoot {
                get {
                    return this;
                }
            }
            
            public virtual void CopyTo(System.Array array, int index) {
                privColObj.CopyTo(array, index);
                int nCtr;
                for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1)) {
                    array.SetValue(new Processor(((System.Management.ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }
            
            public virtual System.Collections.IEnumerator GetEnumerator() {
                return new ProcessorEnumerator(privColObj.GetEnumerator());
            }
            
            public class ProcessorEnumerator : object, System.Collections.IEnumerator {
                
                private ManagementObjectCollection.ManagementObjectEnumerator privObjEnum;
                
                public ProcessorEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum) {
                    privObjEnum = objEnum;
                }
                
                public virtual object Current {
                    get {
                        return new Processor(((System.Management.ManagementObject)(privObjEnum.Current)));
                    }
                }
                
                public virtual bool MoveNext() {
                    return privObjEnum.MoveNext();
                }
                
                public virtual void Reset() {
                    privObjEnum.Reset();
                }
            }
        }
        
        // Elemento TypeConverter que administra valores NULL para propiedades ValueType
        public class WMIValueTypeConverter : TypeConverter {
            
            private TypeConverter baseConverter;
            
            private System.Type baseType;
            
            public WMIValueTypeConverter(System.Type inBaseType) {
                baseConverter = TypeDescriptor.GetConverter(inBaseType);
                baseType = inBaseType;
            }
            
            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type srcType) {
                return baseConverter.CanConvertFrom(context, srcType);
            }
            
            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType) {
                return baseConverter.CanConvertTo(context, destinationType);
            }
            
            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
                return baseConverter.ConvertFrom(context, culture, value);
            }
            
            public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary dictionary) {
                return baseConverter.CreateInstance(context, dictionary);
            }
            
            public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetCreateInstanceSupported(context);
            }
            
            public override PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, System.Attribute[] attributeVar) {
                return baseConverter.GetProperties(context, value, attributeVar);
            }
            
            public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetPropertiesSupported(context);
            }
            
            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValues(context);
            }
            
            public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesExclusive(context);
            }
            
            public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesSupported(context);
            }
            
            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType) {
                if ((baseType.BaseType == typeof(System.Enum))) {
                    if ((value.GetType() == destinationType)) {
                        return value;
                    }
                    if ((((value == null) 
                                && (context != null)) 
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return  "NULL_ENUM_VALUE" ;
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((baseType == typeof(bool)) 
                            && (baseType.BaseType == typeof(System.ValueType)))) {
                    if ((((value == null) 
                                && (context != null)) 
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return "";
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((context != null) 
                            && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                    return "";
                }
                return baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }
        
        // Clase incrustada que representa las propiedades WMI del sistema.
        [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
        public class ManagementSystemProperties {
            
            private System.Management.ManagementBaseObject PrivateLateBoundObject;
            
            public ManagementSystemProperties(System.Management.ManagementBaseObject ManagedObject) {
                PrivateLateBoundObject = ManagedObject;
            }
            
            [Browsable(true)]
            public int GENUS {
                get {
                    return ((int)(PrivateLateBoundObject["__GENUS"]));
                }
            }
            
            [Browsable(true)]
            public string CLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__CLASS"]));
                }
            }
            
            [Browsable(true)]
            public string SUPERCLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__SUPERCLASS"]));
                }
            }
            
            [Browsable(true)]
            public string DYNASTY {
                get {
                    return ((string)(PrivateLateBoundObject["__DYNASTY"]));
                }
            }
            
            [Browsable(true)]
            public string RELPATH {
                get {
                    return ((string)(PrivateLateBoundObject["__RELPATH"]));
                }
            }
            
            [Browsable(true)]
            public int PROPERTY_COUNT {
                get {
                    return ((int)(PrivateLateBoundObject["__PROPERTY_COUNT"]));
                }
            }
            
            [Browsable(true)]
            public string[] DERIVATION {
                get {
                    return ((string[])(PrivateLateBoundObject["__DERIVATION"]));
                }
            }
            
            [Browsable(true)]
            public string SERVER {
                get {
                    return ((string)(PrivateLateBoundObject["__SERVER"]));
                }
            }
            
            [Browsable(true)]
            public string NAMESPACE {
                get {
                    return ((string)(PrivateLateBoundObject["__NAMESPACE"]));
                }
            }
            
            [Browsable(true)]
            public string PATH {
                get {
                    return ((string)(PrivateLateBoundObject["__PATH"]));
                }
            }
        }
    }
}
