var revapi8,
    tpj;    
(function() {           
    if (!/loaded|interactive|complete/.test(document.readyState)) document.addEventListener("DOMContentLoaded",onLoad); else onLoad();  
    function onLoad() {             
        if (tpj===undefined) { tpj = jQuery; if("off" == "on") tpj.noConflict();}
    if(tpj("#rev_slider_8_1").revolution == undefined){
        revslider_showDoubleJqueryError("#rev_slider_8_1");
    }else{
        revapi8 = tpj("#rev_slider_8_1").show().revolution({
            sliderType:"standard",
            //jsFileLocation:"//demo.zozothemes.com/induzy/wp-content/plugins/revslider/public/assets/js/",
            sliderLayout:"fullwidth",
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
                ,
                arrows: {
                    style:"gyges",
                    enable:true,
                    hide_onmobile:false,
                    hide_onleave:true,
                    hide_delay:200,
                    hide_delay_mobile:1200,
                    tmp:'',
                    left: {
                        h_align:"left",
                        v_align:"center",
                        h_offset:20,
                        v_offset:0
                    },
                    right: {
                        h_align:"right",
                        v_align:"center",
                        h_offset:20,
                        v_offset:0
                    }
                }
            },
            responsiveLevels:[1240,1240,778,480],
            visibilityLevels:[1240,1240,778,480],
            gridwidth:[1200,1200,778,480],
            gridheight:[700,700,500,550],
            lazyType:"none",
            parallax: {
                type:"mouse",
                origo:"enterpoint",
                speed:400,
                speedbg:0,
                speedls:0,
                levels:[5,10,15,20,25,30,35,40,45,46,47,48,49,50,51,55],
            },
            shadow:0,
            spinner:"off",
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