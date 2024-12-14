(function($) {

    "use strict";

    /*------------------------slider------------------------*/
    /* https://learn.jquery.com/using-jquery-core/document-ready/ */
    jQuery(document).ready(function() {

        /* initialize the slider based on the Slider's ID attribute from the wrapper above */
        jQuery('#rev_slider_1').show().revolution({
            parallax: {
                type: 'mouse+scroll',
                origo: 'slidercenter',
                speed: 400,
                levels: [5, 10, 15, 20, 25, 30, 35, 40,
                    45, 46, 47, 48, 49, 50, 51, 55
                ],
                disable_onmobile: 'on'
            },

            /* options are 'auto', 'fullwidth' or 'fullscreen' */
            sliderLayout: 'auto',
            /* RESPECT ASPECT RATIO */
            minHeight: '500',
            responsiveLevels: [1170, 1024, 778, 480],
            visibilityLevels: [1170, 1024, 778, 480],
            gridwidth: [1170, 1024, 778, 480],
            gridheight: [780, 780, 860, 720],

            /* basic navigation arrows and bullets */

            /* basic navigation arrows and bullets */
            navigation: {

                arrows: {
                    enable: true,
                    style: "zeus",
                    hide_onleave: false
                },


            }
        });
    });
    /*------------------------slider------------------------*/
    /* https://learn.jquery.com/using-jquery-core/document-ready/ */
    jQuery(document).ready(function() {

        /* initialize the slider based on the Slider's ID attribute from the wrapper above */
        jQuery('#rev_slider_2').show().revolution({
            parallax: {
                type: 'mouse+scroll',
                origo: 'slidercenter',
                speed: 400,
                levels: [5, 10, 15, 20, 25, 30, 35, 40,
                    45, 46, 47, 48, 49, 50, 51, 55
                ],
                disable_onmobile: 'on'
            },

            /* options are 'auto', 'fullwidth' or 'fullscreen' */
            sliderLayout: 'auto',
            /* RESPECT ASPECT RATIO */
            minHeight: '500',
            responsiveLevels: [1170, 1024, 778, 480],
            visibilityLevels: [1170, 1024, 778, 480],
            gridwidth: [1170, 1024, 778, 480],
            gridheight: [885, 780, 860, 720],

            /* basic navigation arrows and bullets */

            /* basic navigation arrows and bullets */
            navigation: {

                arrows: {
                    enable: true,
                    style: "zeus",
                    hide_onleave: false
                },


            }
        });
    });



    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.single_items').owlCarousel({
            loop: true,
            margin: 0,
            nav: true,
            dots: true,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                800: {
                    items: 1
                },

                1000: {
                    items: 1
                },
                1200: {
                    items: 1
                }
            }
        });
    });

    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.single_item_center').owlCarousel({
            loop: true,
            margin: 60,
            nav: false,
            dots: true,
            center: true,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            responsive: {
                0: {
                    items: 1,
                    center: false,
                },
                576: {
                    items: 1,
                    center: false,
                },

                1000: {
                    items: 1
                },
                1200: {
                    items: 1
                }
            }
        });
    });


    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.three_items').owlCarousel({
            loop: false,
            margin: 20,
            nav: true,
            dots: true,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                800: {
                    items: 2
                },

                1000: {
                    items: 2
                },
                1200: {
                    items: 3
                }
            }
        });
    });

    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.three_items_2').owlCarousel({
            loop: true,
            margin: 0,
            nav: true,
            dots: true,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 3
                }
            }
        });
    });

    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.three_items_3').owlCarousel({
            loop: false,
            margin: 30,
            nav: true,
            dots: true,
            center: false,
            smartSpeed: 500,
            autoplay: false,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 3
                }
            }
        });
    });


    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.four_items').owlCarousel({
            loop: true,
            margin: 2,
            nav: true,
            dots: true,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 4
                }
            }
        });
    });

    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.four_items_3').owlCarousel({
            loop: true,
            margin: 50,
            nav: true,
            dots: true,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 4
                }
            }
        });
    });
    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.four_items_2').owlCarousel({
            loop: true,
            margin: 30,
            nav: false,
            dots: false,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 4
                }
            }
        });
    });

    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.four_items_4').owlCarousel({
            loop: true,
            margin: 20,
            nav: false,
            dots: false,
            center: true,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                800: {
                    items: 2
                },
                1000: {
                    items: 2
                },
                1300: {
                    items: 3
                },
                1600: {
                    items: 4,

                }
            }
        });
    });
    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.two_items').owlCarousel({
            loop: true,
            margin: 20,
            nav: true,
            dots: true,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },

                992: {
                    items: 1
                },
                1600: {
                    items: 2
                }
            }
        });
    });

    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.client_logo').owlCarousel({
            loop: true,
            margin: 20,
            nav: false,
            dots: false,
            center: false,
            smartSpeed: 500,
            autoplay: true,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                400: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 4
                }
            }
        });
    });


    /*--------------owl------------------*/
    $(document).ready(function() {
        $('.price_table').owlCarousel({
            loop: true,
            margin: 30,
            nav: false,
            dots: false,
            center: true,
            smartSpeed: 500,
            autoplay: false,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-next"></span>', '<span class="clearfix icon-next"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                800: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 3
                }
            }
        });
    }); /*--------------owl------------------*/
    $(document).ready(function() {
        $('.relt_product').owlCarousel({
            loop: true,
            margin: 30,
            nav: true,
            dots: false,
            center: true,
            smartSpeed: 500,
            autoplay: false,
            autoplayTimeout: 5000,
            navText: ['<span class="clearfix prev icon-keyboard-left-arrow-button"></span>', '<span class="clearfix icon-keyboard-right-arrow-button"></span>'],

            responsive: {
                0: {
                    items: 1
                },
                800: {
                    items: 2
                },

                1000: {
                    items: 3
                },
                1600: {
                    items: 3
                }
            }
        });
    });


    /*---------------------------scroll-to-top---------------------------------*/
    $(function() {
        $(document).ready(function() {
            $(window).scroll(function() {
                if ($(this).scrollTop() > 100) {
                    $('#scroll').fadeIn();
                } else {
                    $('#scroll').fadeOut();
                }
            });
            $('#scroll').click(function() {
                $("html, body").animate({ scrollTop: 0 }, 2000);
                return false;
            });
        });
    });

    jQuery(window).scroll(startCounter);

    function startCounter() {

        if (jQuery(window).scrollTop() > 0) {
            jQuery(window).off("scroll", startCounter);
            jQuery('.counter-value').each(function() {
                var $this = jQuery(this);
                jQuery({ Counter: 0 }).animate({ Counter: $this.text() }, {
                    duration: 2000,
                    easing: 'swing',
                    step: function() {
                        $this.text(Math.ceil(this.Counter) + '');
                    }
                });
            });
        }
        if (jQuery(window).scrollTop() > 0) {
            jQuery(window).off("scroll", startCounter);
            jQuery('.counter-value-plus').each(function() {
                var $this = jQuery(this);
                jQuery({ Counter: 0 }).animate({ Counter: $this.text() }, {
                    duration: 2000,
                    easing: 'swing',
                    step: function() {
                        $this.text(Math.ceil(this.Counter) + '+');
                    }
                });
            });
        }
    }

    // jquery ui select menu active js
    $(function() {
        $("#service").selectmenu();
        $("#state").selectmenu();
        $("#datepicker").datepicker();

    });
    // external js: masonry.pkgd.js, imagesloaded.pkgd.js
    // init Masonry
    var $grid = $('.gridtwo').masonry({
        itemSelector: '.grid-item',
        percentPosition: true,
        columnWidth: '.grid-sizer',


    });
    // layout Masonry after each image loads
    $grid.imagesLoaded().progress(function() {
        $grid.masonry();
    });


    var $grid = $('.gridthree').masonry({
        itemSelector: '.grid-item',
        percentPosition: true,
        columnWidth: '.grid-sizer',
        gutter: 20
    });
    // layout Masonry after each image loads
    $grid.imagesLoaded().progress(function() {
        $grid.masonry();
    });


    // preloader js
    function prealoader() {
        if ($('.preloader').length) {
            $('.preloader').delay(100).fadeOut(500);
        }
    }

    jQuery(window).on('load', function() {
        (function($) {
            prealoader();
        })(jQuery);
    });

})(window.jQuery);