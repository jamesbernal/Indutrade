
// Animated Horizontal Submenu

	$(document).ready(function(){
		var link = $('.menu > ul > li');
		$(link).on('mouseover', function() {
			$(this).addClass('active').siblings().removeClass('active');
		});
	
	});