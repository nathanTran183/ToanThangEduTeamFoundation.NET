var _banner_slide_time = 4000;
var _banner_slide_transion_time = 1000;
var _banner_slide_item;
var _banner_slide_current = 0;

$(document).ready(function(){
	_banner_slide_item = $('.banner_slide img');
	if (_banner_slide_item.length > 0) {
		$(_banner_slide_item).hide();
		$(_banner_slide_item[0]).show();
		
		if (_banner_slide_item.length > 1) {
			setInterval("slide_banner()", _banner_slide_time);
		}
	}
	
	$('.wrapper-dropdown span').click(function(){
		$('.wrapper-dropdown').removeClass('active');
	});
});

function slide_banner() {
	$(_banner_slide_item[_banner_slide_current]).fadeOut(_banner_slide_transion_time);
	_banner_slide_current += 1;
	if (_banner_slide_current == _banner_slide_item.length) 
		_banner_slide_current = 0;
	$(_banner_slide_item[_banner_slide_current]).fadeIn(_banner_slide_transion_time);
}