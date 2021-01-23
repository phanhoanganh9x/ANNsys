// show popup
function showPopup(content, col) {
    if (!col)
        col = 8;

    let obj = $('body');

    $(obj).attr('onkeydown', 'keyclose_ms(event)');

    let bg = '<div id="bg_popup"></div>';
    let fr = '';

    fr += '<div id="pupip" class="columns-container1">';
    fr += '    <div class="container" id="columns">';
    fr += '        <div class="row">';
    fr += '            <div class="center_column col-xs-12 col-sm-' + col + '" id="popup_content">';
    fr += '                <a onclick="closePopup()" class="close_message"></a>';
    fr += '                <div class="content-popup scrollbar">';
    fr += content;
    fr += '                </div>';
    fr += '            </div>';
    fr += '         </div>'
    fr += '     </div>';
    fr += '</div>';

    $(bg).appendTo($(obj)).show().animate({
        "opacity": 0.7
    }, 0);
    $(fr).appendTo($(obj));
}

// close popup when press escape
function keyclose_ms(e) {
    if (e.keyCode === 27) {
        closePopup();
    }
}

// close popup
function closePopup() {
    $("#pupip").remove();
    $("#bg_popup").remove();
}