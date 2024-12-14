var revapi7,
		tpj;	
	(function() {			
		if (!/loaded|interactive|complete/.test(document.readyState)) document.addEventListener("DOMContentLoaded",onLoad); else onLoad();	
		function onLoad() {				
			if (tpj===undefined) { tpj = jQuery; if("off" == "on") tpj.noConflict();}
		if(tpj("#rev_slider_7_1").revolution == undefined){
			revslider_showDoubleJqueryError("#rev_slider_7_1");
		}else{
			revapi7 = tpj("#rev_slider_7_1").show().revolution({
				sliderType:"standard",
				//jsFileLocation:"//demo.zozothemes.com/induzy/wp-content/plugins/revslider/public/assets/js/",
				sliderLayout:"auto",
				dottedOverlay:"none",
				delay:9000,
				navigation: {
					keyboardNavigation:"off",
					keyboard_direction: "horizontal",
					mouseScrollNavigation:"off",
								mouseScrollReverse:"default",
					onHoverStop:"off",
					touch:{
						touchenabled:"on",
						touchOnDesktop:"on",
						swipe_threshold: 75,
						swipe_min_touches: 1,
						swipe_direction: "horizontal",
						drag_block_vertical: false
					}
				},
				responsiveLevels:[1240,1024,778,480],
				visibilityLevels:[1240,1024,778,480],
				gridwidth:[1370,1024,778,480],
				gridheight:[700,450,400,350],
				lazyType:"none",
				shadow:0,
				spinner:"spinner1",
				stopLoop:"off",
				stopAfterLoops:-1,
				stopAtSlide:-1,
				shuffle:"off",
				autoHeight:"off",
				disableProgressBar:"on",
				hideThumbsOnMobile:"off",
				hideSliderAtLimit:0,
				hideCaptionAtLimit:0,
				hideAllCaptionAtLilmit:0,
				debugMode:false,
				fallbacks: {
					simplifyAll:"off",
					nextSlideOnWindowFocus:"on",
					disableFocusListener:false,
				}
			});
		}; /* END OF revapi call */
	 }; /* END OF ON LOAD FUNCTION */
	}()); /* END OF WRAPPING FUNCTION */