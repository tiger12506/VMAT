function incrementUp(obj, val) {
    var value = val.valueOf;
    value++;
    obj.Text = value;
}

function incrementDown(obj, val) {
    var value = val.valueOf;
    value--;
    obj.Text = value;
}
