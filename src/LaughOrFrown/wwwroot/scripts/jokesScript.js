$(document).ready(function () {
    $("#leftArrow").on("mouseenter", function () {
        $(".leftText").animate({
            fontSize:"2.7em"
        },300);
    });
    $("#leftArrow").on("mouseleave", function () {
        $(".leftText").stop();
        $(".leftText").css("font-size", "");
    });

    $("#rightArrow").on("mouseenter", function () {
        $(".rightText").animate({
            fontSize: "2.7em"
        }, 300);
    });
    $("#rightArrow").on("mouseleave", function () {
        $(".rightText").stop();
        $(".rightText").css("font-size", "");
    });


});