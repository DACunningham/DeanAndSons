tinymce.init({
    selector: '#titleDisp',
    inline: true,
    toolbar: 'undo redo',
    forced_root_block : 'div',
    menubar: false
});

tinymce.init({
    selector: '#descriptionDisp',
    inline: true,
    forced_root_block : 'div',
    plugins: [
      'advlist autolink lists link image charmap print preview anchor',
      'searchreplace visualblocks code fullscreen',
      'insertdatetime media table contextmenu paste'
    ],
    toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image'
});