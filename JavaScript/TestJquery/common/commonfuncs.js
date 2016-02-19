/* File Created: July 20, 2012 */

function wrap4display(value) {
    var displayvalue = value;
    if (typeof value == "string") {
        displayvalue = "'" + value + "'";
    } else if (value instanceof Array) {
        displayvalue = "[" + value + "]";
    }
    return displayvalue;
}