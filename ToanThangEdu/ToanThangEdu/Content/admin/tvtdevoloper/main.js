;
(function ($) {

	hotline();

	$(".icon-mobile-menu").click(function (e) {
		e.preventDefault()
		$("nav").slideToggle("fast", function () {
			// Animation complete.
		});
	});

	var allPanels = $('.acc-content').hide(),
			allHeaders = $('.acc-item');

	allHeaders.click(function () {
		$this = $(this);
		$target = $this.next();
		// Remove all header active class
		allHeaders.removeClass('active');
		$this.addClass('active');
		if (!$target.hasClass('active')) {
			allPanels.removeClass('active').hide();
			$target.addClass('active').show();
		}
		return false;
	}).eq(0).trigger('click');

	$(function () {
		var dd = new DropDown($('#game-service')),
				bb = new DropDown($('#faq-popular'));

		$(document).click(function () {
			$('.wrapper-dropdown').removeClass('active');
		});
	});

	ctrlMenu('full-list', 'faq-list', 'faq-content-block');
	faqCtrl();

})(jQuery);

function hotline() {
//	var box = $('.hotline-number'),
//			btn = box.find('span'),
//			number = box.find('.content'),
//			hFunction = $('.hotline-function');
//
//	btn.on("click", function () {
//		if (btn.hasClass('down')) {
//			btn.removeClass('down');
//			btn.addClass('up');
//			hFunction.stop().animate({
//				"height": "auto",
//				"opacity": 1
//			});
//		} else {
//			btn.removeClass('up');
//			btn.addClass('down');
//			hFunction.stop().animate({
//				"height": 0,
//				"opacity": 0
//			});
//		}
//	});
}

function DropDown(el) {
	this.dd = el;
	this.placeholder = this.dd.children('span');
	this.opts = this.dd.find('ul.dropdown > li');
	this.val = '';
	this.index = -1;
	this.initEvents();
}
DropDown.prototype = {
	initEvents: function () {
		var obj = this;

		obj.dd.on('click', function (event) {
			$(this).toggleClass('active');
			return false;
		});

		obj.opts.on('click', function () {
			var opt = $(this);
			obj.val = opt.text();
			obj.index = opt.index();
			obj.placeholder.text(obj.val);
		});
	},
	getValue: function () {
		return this.val;
	},
	getIndex: function () {
		return this.index;
	}
}

function ctrlMenu(cl1, cl2, cl3) {
	var cItem = $('.' + cl1),
			sItem = $('.' + cl2),
			cDetail = $('.' + cl3);
	cItem.on('click', function () {
		if (!sItem.hasClass('show')) {
			sItem.addClass('show');
			cDetail.addClass('hide');
		} else {
			sItem.removeClass('show');
			cDetail.removoClass('hide');
		}
	})
}

function faqCtrl() {
	var nav = $('.faq-list'),
			nList = nav.find('li');
	
	nList.on("click", function () {
		var txt = $(this).find('span').text();
		$('.title-faq').text(txt);
		$('.faq-content-block').removeClass('hide');
	});
}
