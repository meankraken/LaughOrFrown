$(document).ready(function () {
    //add the current date on index page
    var now = new Date();
    var months = ['Jan.', ',Feb.', 'Mar.', 'Apr.', 'May', 'June', 'July', 'Aug.', 'Sept.', 'Oct.', 'Nov.', 'Dec.'];
    var dateStr = months[now.getMonth()] + " " + now.getDate() + ", " + now.getFullYear();
    $('#dateText').append("<span>" + dateStr + "</span>"); 

    //handle hover for menu buttons
    $('img').hover(function () { 
        $(this).css('filter', 'grayscale(0)');
        $(this).css('-webkit-filter', 'grayscale(0%)');

    }, function () {
        $(this).css('filter', '');
        $(this).css('-webkit-filter', '');
    });

    //expand page if showing user panel
    var userRowExists = $('#userPanel'); 
   
    if (!(userRowExists.length == 0)) {
        $('.container').css('height', '1250px');
    }

    //handle submit joke form
    var jokeForm = $('.jokeSubmitContainer');

    $('#addJokeBtn').on('click', function (e) {
        e.stopPropagation();
        $('.mask').css('display', 'block');
        $('.jokeSubmitContainer').css('display', 'block');
        $('#jokeForm').focus();
        
    });

    $('#cancelBtn').on('click', function () {
        $('.mask').css('display', 'none');
        $('.jokeSubmitContainer').css('display', 'none');
    });

    $('#jokeForm').on('submit', function (e) { //validate form inputs 
        $('#jokeTextError').css('display', 'none');
        $('#punchlineError').css('display', 'none');
        if ($('#jokeText').val().length < 5 || $('#jokeText').val().length > 50) {
            $('#jokeTextError').css('display', 'inline');
            e.preventDefault();
        }
        else if ($('#punchline').val().length < 1 || $('#punchline').val().length > 255) {
            $('#punchlineError').css('display', 'inline');
            e.preventDefault();
        }
        
    });

    $(document).bind('click', function (e) {
        if (!(e.target.className == 'jokeSubmitContainer') && (jokeForm.has(e.target).length === 0)) {
            $('.mask').css('display', 'none');
            $('.jokeSubmitContainer').css('display', 'none');
        }
               
    });
    
    



});