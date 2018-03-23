employer.util = {
    /// <summary>
    /// Recupera o controle desejado.
    /// </summary>
    findControl: function (control) {
        return $('[id$=' + control + ']');
    },
    /// <summary>
    /// Remove espaços a direita e a esquerda da string.
    /// </summary>
    trim : function Trim(valor)
    {
        return valor.replace(/^\s+|\s+$/g, '');
    }
}