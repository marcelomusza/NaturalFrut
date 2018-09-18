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

