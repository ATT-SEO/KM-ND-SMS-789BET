//tinymce.PluginManager.add('image', function (editor, url) {
//    function createImageList() {
//        return function () {
//            editor.windowManager.openUrl({
//                title: 'Chèn hình ảnh',
//                url: '/admin/editor/image',
//                width: 800,
//                height: 550,
//            });
//        };
//    }
//    function createSlideList() {
//        return function () {
//            // Your existing code for creating a slide list
//        };
//    }

//    // Define other functions (createBox_2_image, createBox_3_image) similarly

//    editor.ui.registry.addButton('image', {
//        icon: 'image',
//        tooltip: 'Chèn hình ảnh',
//        onAction: createImageList(),
//    });

//    editor.ui.registry.addMenuItem('image', {
//        icon: 'image',
//        text: 'Chèn hình ảnh',
//        onAction: createImageList(),
//    });
//});
tinymce.PluginManager.add('media', function (editor, url) {
    function createMediaList() {
        return function () {
            editor.windowManager.openUrl({
                title: 'Chèn video',
                url: '/Tinymce/Video',
                width: 800,
                height: 550
            });
        };
    }

    editor.ui.registry.addButton('media', {
        icon: 'media',
        tooltip: 'Chèn video',
        onAction: createMediaList(),
    });

    editor.ui.registry.addMenuItem('media', {
        icon: 'media',
        text: 'Chèn video',
        onAction: createMediaList(),
    });

});