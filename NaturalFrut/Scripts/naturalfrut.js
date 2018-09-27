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

