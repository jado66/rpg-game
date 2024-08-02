mergeInto(LibraryManager.library, {
  SaveToLocalStorage: function (key, value) {
    localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
  },

  LoadFromLocalStorage: function (key) {
    var value = localStorage.getItem(UTF8ToString(key));
    var bufferSize = lengthBytesUTF8(value) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(value, buffer, bufferSize);
    return buffer;
  },

  RemoveFromLocalStorage: function (key) {
    localStorage.removeItem(UTF8ToString(key));
  },
});
