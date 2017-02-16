( function( $ ) {
    $( document ).ready(function() {
        //$('#cssmenu > ul').prepend('<li style=\"   height:50px;width:50px;\" class=\"mobile\"><a href=\"#\"></a></li>');
        //$('#cssmenu > ul').prepend('<li style=\"   height:46px;width:46px;\" class=\"mobile\"><a href=\"#\">/*<span><i>&#nbsp;</i></span>*/</a></li>');

        $('#cssmenu > ul > li > a').click(function(e) {
          $('#cssmenu li').removeClass('active');
          $(this).closest('li').addClass('active');	
          var checkElement = $(this).next();
          if((checkElement.is('ul')) && (checkElement.is(':visible'))) {
            $(this).closest('li').removeClass('active');
            checkElement.slideUp('normal');
          }
          if((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
            $('#cssmenu ul ul:visible').slideUp('normal');
            checkElement.slideDown('normal');
          }
          if( $(this).parent().hasClass('mobile') ) {
            e.preventDefault();
            $('#cssmenu').toggleClass('expand');
          }
          if($(this).closest('li').find('ul').children().length == 0) {
            return true;
          } else {
            return false;	
          }		
        });


        $('#menu').click(function (e) {
            if ($('#cont_menu').is(':hidden'))
                $("#cont_menu").fadeToggle(1000);
            else
                $('#cont_menu').hide();
        });
    });
})( jQuery );
