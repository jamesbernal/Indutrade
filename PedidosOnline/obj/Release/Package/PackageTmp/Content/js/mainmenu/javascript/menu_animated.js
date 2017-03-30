


// Animated Menu


$(document).ready(function () {
    $(".abuttons").click(function () {
        $(this).css('background-color', 'black');
        var a = $(this).attr("href");
        var btn = $(this).attr("name");
        document.getElementById('navbar').style.display = 'none';
        document.getElementById('content').style.display = 'none';

        if (btn == "btncrm") {
            var v_obj = document.getElementsByName('btnventas');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnautoservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnerp');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnconfig');
            v_obj.setAttribute('style', 'background-color:transparent');
            $('#tab6').hide();
            $('#tab5').hide();
            $('#tab4').hide();
            $('#tab3').hide();
            $('#tab2').hide();
        }
        else if (btn == "btnventas") {
           
            var v_obj = document.getElementsByName('btncrm');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnautoservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnerp');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnconfig');
            v_obj.setAttribute('style', 'background-color:transparent');
            $('#tab1').hide();
            $('#tab3').hide();
            $('#tab4').hide();
            $('#tab5').hide();
            $('#tab6').hide();
        }
        else if (btn == "btnservicio") {
           
            var v_obj = document.getElementsByName('btncrm');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnventas');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnautoservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnerp');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnconfig');
            v_obj.setAttribute('style', 'background-color:transparent');
            $('#tab1').hide();
            $('#tab2').hide();
            $('#tab4').hide();
            $('#tab5').hide();
            $('#tab6').hide();
        }
        else if (btn == "btnautoservicio") {
         
            var v_obj = document.getElementsByName('btncrm');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnventas');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnerp');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnconfig');
            v_obj.setAttribute('style', 'background-color:transparent');
            $('#tab1').hide();
            $('#tab2').hide();
            $('#tab3').hide();
            $('#tab5').hide();
            $('#tab6').hide();
        }
        else if (btn == "btnerp") {
            var v_obj = document.getElementsByName('btncrm');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnventas');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnautoservicio');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj = document.getElementsByName('btnconfig');
            v_obj.setAttribute('style', 'background-color:transparent');
            $('#tab1').hide();
            $('#tab2').hide();
            $('#tab3').hide();
            $('#tab4').hide();
            $('#tab6').hide();
        }
        else if (btn == "btnconfig") {
            var v_obj = document.getElementsByName('btncrm');
            v_obj.setAttribute('style', 'background-color:transparent');
            var v_obj2 = document.getElementsByName('btnventas');
            v_obj2.setAttribute('style', 'background-color:transparent');
            var v_obj3 = document.getElementsByName('btnservicio');
            v_obj3.setAttribute('style', 'background-color:transparent');
            var v_obj4 = document.getElementsByName('btnerp');
            v_obj4.setAttribute('style', 'background-color:transparent');
            var v_obj5 = document.getElementsByName('btnautoservicio');
            v_obj5.setAttribute('style', 'background-color:transparent');
            $('#tab1').hide();
            $('#tab2').hide();
            $('#tab3').hide();
            $('#tab4').hide();
            $('#tab5').hide();

        }
    });
});