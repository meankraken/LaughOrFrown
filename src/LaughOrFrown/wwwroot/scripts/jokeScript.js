$(document).ready(function () {
    var hotCaption = $('#caption1');
    var offenseCaption = $('#caption2');

    var hotStartingValue = $('#hotSelect').val();
    var offStartingValue = $('#offensiveSelect').val();

    $('#hotSelect').change(function () {
        if (hotStartingValue === $('#hotSelect').val() && offStartingValue === $('#offensiveSelect').val()) { //ensure the selection has changed before enabling submit btn 
            $('#rateBtn').attr('disabled', true);
        }
        else {
            $('#rateBtn').attr('disabled', false);
        }

        switch (parseInt($(this).val())) {
            case 1:
                hotCaption.text("[Not even a smirk]");
                break;
            case 2:
                hotCaption.text("[Dad joke]");
                break;
            case 3:
                hotCaption.text("[Smiled a little]");
                break;
            case 4:
                hotCaption.text("[Now that's funny]");
                break;
            case 5:
                hotCaption.text("[The coveted out loud laugh]");
                break;
            default:
                hotCaption.text("");
        }
    });

    $('#offensiveSelect').change(function () {
        if (hotStartingValue === $('#hotSelect').val() && offStartingValue === $('#offensiveSelect').val()) {
            $('#rateBtn').attr('disabled', true);
        }
        else {
            $('#rateBtn').attr('disabled', false);
        }

        switch (parseInt($(this).val())) {
            case 1:
                offenseCaption.text("[Just downright pleasant]");
                break;
            case 2:
                offenseCaption.text("[That's sweet]");
                break;
            case 3:
                offenseCaption.text("[A little messed up...]");
                break;
            case 4:
                offenseCaption.text("[Now that's edgy]");
                break;
            case 5:
                offenseCaption.text("[I should probably report this]");
                break;
            default:
                offenseCaption.text("");
        }
    });

});