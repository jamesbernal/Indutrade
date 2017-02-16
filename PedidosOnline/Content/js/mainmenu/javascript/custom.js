$(document).ready(function () {
 
    // function to show our popups
    function showPopup(whichpopup) {
        var docHeight = $(document).height(); //grab the height of the page
        var scrollTop = $(window).scrollTop(); //grab the px value from the top of the page to where you're scrolling

        if (whichpopup == 2) {

            if ($("#navbar").is(":visible")) {
                $('.popup' + whichpopup).show().css({ 'top': scrollTop + -25 + 'px' });
            } else {
                $('.popup' + whichpopup).show().css({ 'top': scrollTop + 25 + 'px' });
            }


            if ($("#crm").is(":visible")) {
                document.getElementById("crm").style.top = "-154px";
            }

            if ($("#ventas").is(":visible")) {
                document.getElementById("ventas").style.top = "-154px";
            }

            if ($("#servicio").is(":visible")) {
                document.getElementById("servicio").style.top = "-154px";
            }

            if ($("#autoservicio").is(":visible")) {
                document.getElementById("autoservicio").style.top = "-154px";
            }
            //var display5 = document.getElementById("erp").style.display;
            if ($("#erp").is(":visible")) {
                document.getElementById("erp").style.top = "-154px";
            }

            //var display6 = document.getElementById("config").style.display;
            //if ($("#config").is(":visible")) {
            //    document.getElementById("config").style.top = "-154px";
            //}
        }
        else {

            $('.popup' + whichpopup).show().css({ 'top': scrollTop + 20 + 'px' }); //show the appropriate popup and set the content 20px from the window top

        }

    }
    // function to close our popups
    function closePopup() {
        $('.overlay-content').hide(); //hide the overlay

    }

    // timer if we want to show a popup after a few seconds.
    //get rid of this if you don't want a popup to show up automatically
    setTimeout(function () {
        // Show popup3 after 2 seconds
        showPopup(3);
    }, 2000);


    // show popup when you click on the link
    $('.show-popup').click(function (event) {
        event.preventDefault(); // disable normal link function so that it doesn't refresh the page
        var selectedPopup = $(this).data('showpopup'); //get the corresponding popup to show

        showPopup(selectedPopup); //we'll pass in the popup number to our showPopup() function to show which popup we want

    });
    $('body').bind('click', function (e) {
        var elm = window.event.srcElement.className;


        if ($(e.target).closest('.show-popup').length == 0 || $('.' + elm + '').closest('.show-popup').length == 0) {
            closePopup();
            if ($("#crm").is(":visible")) {
                document.getElementById("crm").style.top = "-46px";
            }

            if ($("#ventas").is(":visible")) {
                document.getElementById("ventas").style.top = "-46px";
            }

            if ($("#servicio").is(":visible")) {
                document.getElementById("servicio").style.top = "-46px";
            }

            if ($("#autoservicio").is(":visible")) {
                document.getElementById("autoservicio").style.top = "-46px";
            }
            var display5 = document.getElementById("erp").style.display;
            if ($("#erp").is(":visible")) {
                document.getElementById("erp").style.top = "-46px";
            }
            //var display6 = document.getElementById("config").style.display;
            //if ($("#config").is(":visible")) {
            //    document.getElementById("config").style.top = "-46px";
            //}

        }
    });
    // hide popup when user clicks on close button or if user clicks anywhere outside the container


    // hide the popup when user presses the esc key
    $(document).keyup(function (e) {
        if (e.keyCode == 27) { // if user presses esc key
            closePopup();
        }
    });
});