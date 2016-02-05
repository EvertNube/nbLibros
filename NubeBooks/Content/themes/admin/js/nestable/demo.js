$(document).ready(function()
{

    // activate Nestable for list 1
    $('#nestable1').nestable({
        group: 1,
        maxDepth: 4
    });
    
    var $expand = false;
    $('#nestable-menu').on('click', function(e)
    {
        if ($expand) {
            $expand = false;
            $('.dd').nestable('expandAll');
        }else {
            $expand = true;
            $('.dd').nestable('collapseAll');
        }
    });
    $('#nestable-menu').click();

});