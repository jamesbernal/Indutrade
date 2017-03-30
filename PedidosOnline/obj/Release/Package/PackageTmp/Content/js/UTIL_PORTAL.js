function LoadView(urlToNavigate) {
        //Incluir icono de cargando
        $("#content_external").html('<p align=center><img src="/Images/loading.gif" /></p>');
        $("#content_external").load(urlToNavigate);  
}