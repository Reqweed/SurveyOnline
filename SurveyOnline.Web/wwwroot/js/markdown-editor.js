const markdownEditor = new EasyMDE({
    element: document.getElementById('markdown-editor'),
    previewRender: function(plainText) {
        return markdownEditor.markdown(plainText);
    }
});