var bne = {};
bne.components = {};

bne.components.web = {
    /// <summary>
    /// Retorna a versão da Framework.
    /// </summary>
    VERSION: function () {
        return 1.0;
    },
    // MMDDYYYY
    VERSION_DATE: function () {
        /// <summary>
        /// Retorna a data de alteração da versão da Framework.
        /// </summary>    
        return new Date("25-04-2012");
    }
};