$(document).ready(function () {
    var now = new Date();
    var months = ['Jan.', ',Feb.', 'Mar.', 'Apr.', 'May', 'June', 'July', 'Aug.', 'Sept.', 'Oct.', 'Nov.', 'Dec.'];
    var dateStr = months[now.getMonth()] + " " + now.getDate() + ", " + now.getFullYear();
    $('#dateText').append("<span>" + dateStr + "</span>");

    $('img').hover(function () {
        $(this).css('filter', 'grayscale(0)');
        $(this).css('-webkit-filter', 'grayscale(0%)');

    }, function () {
        $(this).css('filter', '');
        $(this).css('-webkit-filter', '');
    });

});