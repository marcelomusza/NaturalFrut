//Funcion para formatear los valores monetarios y convertirlos en un numero
function FormatearValorMonetario(valor) {
       
    if (valor.indexOf("$") != -1) {
        valor = valor.replace("$", "").trim();
    }

    if (valor.indexOf(".") != -1) {
        valor = valor.replace(/\./g, "");
    }
        
    return valor;
}

function RemoverSimboloPeso(valor) {

    if (valor.indexOf("$") != -1) {
        valor = valor.replace("$", "").trim();        
    }

    //if (valor.indexOf(",") != -1) {
    //    if (valor.indexOf(".") != -1) {
    //        valor = valor.replace(/\./g, "");
    //    }
    //}
    

    return valor;
}

function ValorANumero(v) {

    if (!v) { return 0; }
    v = v.split('.').join('');
    v = v.split(',').join('.');

    return Number(v.replace(/[^0-9.]/g, ""));
}

function FormatearValor(valor) {

    var formatter = new Intl.NumberFormat('es-AR', {
        style: 'currency',
        currency: 'ARS',
        //minimumFractionDigits: 2,
        // the default value for minimumFractionDigits depends on the currency
        // and is usually already 2
    });

    valor = formatter.format(valor);

    return valor;

}

function DeFormatearValor(valor) {

    if (valor.indexOf("$") != -1 || $("#descuento").val() != "") {
        valor = valor.replace("$", "").trim();
        valor = ValorANumero(valor);
    }

    return valor;
}

function ValidarPunto(textbox) {

    var word = ["."];

    for (i = 0; i < word.length; i++) {
        if (textbox.value.indexOf(word[i]) >= 0) {
            $("#" + textbox.id).val($("#" + textbox.id).val().replace(".", ","));
        }
    }

}
