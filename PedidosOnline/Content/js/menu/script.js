( function( $ ) {
    $( document ).ready(function() {   
        $('#menu').click(function (e) {
            if ($('#cssmenu').is(':hidden'))
                $("#cssmenu").fadeToggle(1000);
            else {
                $(".subopcionesmenu").hide();
                $(".subopciones").hide();
                $('#cssmenu').hide();
            }
        });

        //$("#menu")
        //    .mouseover(function () {
        //        $("#cssmenu").fadeToggle(1000);
        //    })
        //    .mouseout(function () {

        //    });

        //$("#container")
        //    .mouseover(function () {
        //        $(".subopcionesmenu").hide();
        //        $(".subopciones").hide();
        //        $('#cssmenu').hide();
        //    });

        //$("footer")
        //    .mouseover(function () {
        //        $(".subopcionesmenu").hide();
        //        $(".subopciones").hide();
        //        $('#cssmenu').hide();
        //    });



        $("#container")
            .click(function () {
                $(".subopcionesmenu").hide();
                $(".subopciones").hide();
                $('#cssmenu').hide();
            });

        $("#footer")
            .click(function () {
                $(".subopcionesmenu").hide();
                $(".subopciones").hide();
                $('#cssmenu').hide();
            });

        $('#cuenta').click(function (e) {
            if ($("#info_cuenta").is(":visible"))
                $('#info_cuenta').hide();
            else 
                $('#info_cuenta').fadeToggle(1000);            
        });

        $('#container').click(function (e) {
            $('#cssmenu').hide();
            $('#info_cuenta').hide();
            $(".subopcionesmenu").hide();
            $(".subopciones").hide();
        });
    });
})( jQuery );

