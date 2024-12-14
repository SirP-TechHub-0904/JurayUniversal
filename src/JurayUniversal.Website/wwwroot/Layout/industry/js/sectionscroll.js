
/*
 * Section scroll library
 * Author: Zozothemes
 * Version: 1.0
 */
(function( $ ) {
	
	$.fn.sectionscroll = function( options ) {
		
		var frame = $(options.frame),
		container = $(options.container),
		sections = $(options.sections),
		speed = options.speed,
		easing = options.easing;
		
		zoacres_scroll_animation( 0, container );
		
		var didScroll = true,
			isFocused = true;

		// common variables
		var height = $(window).height();

		// Index values for sections elements
		var totalSections = sections.length - 1;

		// currently displayed section number
		// modifying this variable will cause buggy behaviors.
		var num = 0; 

		// keyboard input values
		// add more if necessary
		var pressedKey = {};
			pressedKey[36] = "top"; // home
			pressedKey[38] = "up"; // upward arrow
			pressedKey[40] = "down"; // downward arrow
			pressedKey[33] = "up"; // page up
			pressedKey[34] = "down"; // page down
			pressedKey[35] = "bottom"; // end
			
		// init function to initialize/reassign values of the variables to prevent section misplacement caused by a window resize.
		function init(){
			height = $(window).height();
			frame.css({"overflow":"hidden", "height": height + "px"});
			sections.css({"height": height + "px"});
			didScroll = true;
			isFocused = true;
			end = - height * ( totalSections );

			
			container.stop().animate({marginTop : 0}, 0, easing, function(){
				num = 0;
				didScroll = true;
			});
		}
		// event binding to init function
		$(window).bind("load resize", init);
		
		// animate scrolling effect
		var now, end;
		function animateScr(moveTo, duration, distance){
		
			var top;
			//duration = duration || speed;
			duration = speed;
			switch(moveTo){
				case "down":
					top = "-=" + ( height * distance ) + "px";
					num += distance;
					break;
				case "up":
					top = "+=" + ( height * distance ) + "px";
					num -= distance;
					break;
				case "bottom":
					top = end;
					num = totalSections;
					break;
				case "top":
					top = 0;
					num = 0;
					break;
				default: console.log("(error) wrong argument passed"); return false;
			}
			
			zoacres_scroll_animation( num, container )
			
			container.not(":animated").animate({marginTop : top}, duration, easing, function(){
				didScroll = true;
				//Force Appear Custom Code
			});
		}

		/*	
			1. get a type of event and handle accordingly
			2. enable/disable default keyboard behavior
		*/
		$(document).bind("DOMMouseScroll mousewheel keydown", function(e){
			var eType = e.type;

			now = parseInt( container.css("marginTop") );
			end = - height * ( totalSections );
			
			// handles the event
			if( didScroll && isFocused ){
				// prevent multiple event handling
				didScroll = false;

				// on wheel
				if( eType == "DOMMouseScroll" || eType == "mousewheel" ){

					var mvmt = e.originalEvent.wheelDelta;
					if(!mvmt){ mvmt = -e.originalEvent.detail; }

					if(mvmt > 0){
						if( now == 0){
							didScroll = true;
						}else{
							animateScr("up", 500, 1);
						}
					}else if(mvmt < 0){
						if( now == end ){
							didScroll = true;
						}else{
							animateScr("down", 500, 1);
						}
					}else{
						didScroll = true; 
					}
				}
				// on keydown
				else if( eType == "keydown" ){
					if( pressedKey[e.which] ){
						e.preventDefault();
						if( pressedKey[e.which] == "up" ){
							if( now == 0 ){
								animateScr("bottom");
							}else{
								animateScr("up", speed, 1);
							}
						}else if( pressedKey[e.which]  == "down" ){
							if( now == end ){
								animateScr("top");
							}else{
								animateScr("down", speed, 1);
							}
						}else{
							animateScr( pressedKey[e.which] );
						}
					}else{
						didScroll = true;
					}
				}
			}

			// enable default keyboard behavior when an input or textarea is being focused
			$("input, textarea").focus(function(){isFocused = false;})
								.blur(function(){isFocused = true;});

		});
		
	};

	function zoacres_scroll_animation( num, container ){
		num++;
		$(container).find("section").removeClass("animating");
		$(container).find("section:nth-child("+ num +")").addClass("animating");
	}

	
})( jQuery );