$(document).ready(function () {
    var menu = $('#popUpMenu');
    var currentJoke = 0;

    $('.bodyRow').on('click', function (e) { //bring up action menu 
        e.stopPropagation();
        menu.css('display', 'none'); 
        currentJoke = $(this).attr('id');
        //menu.css('display', 'block'); 
        menu.fadeIn();
        menu.css('left', e.pageX - 50);
        menu.css('top', e.pageY);
    });

    $('#viewJoke').on('click', function () { //go to joke view
        window.location = '/App/Joke/' + currentJoke + '?returnurl=%2FProfile%2FJokes';  

    });

    $('#deleteJoke').on('click', function () { //submit form to delete the joke 
        var result = confirm("Are you sure you want to delete this masterpiece?");
        if (result) {
            $('#jokeid').val(currentJoke);
            $('#deleteForm').submit();
        }
    });

    $(document).on('click', function (e) {
        if (!$(e.target).hasClass('bodyRow')) {
            menu.css('display', 'none');
        }
    });

});