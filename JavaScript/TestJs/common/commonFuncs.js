
// chekanote: by using this method, include this "commonFuns.js" will automatically
// reference other libraries
document.writeln('<script type="text/javascript" src="../common/sprintf.js"></script>');

function wrapQuotation(value) {
    var displayvalue = value;
    if (typeof value == "string") {
        displayvalue = "'" + value + "'";
    }
    return displayvalue;
}

function formatTwoParmExpression(x, operator, y, resultin, answer) {
    return "<li><span class='parameter'>" + wrapQuotation(x) + "</span> " + operator + " <span class='parameter'>" + wrapQuotation(y) + "</span> " + resultin + " <span class='result'>" + wrapQuotation(answer) + "</span></li>";
}

function printline(value) {
    document.writeln(value + "<br/>");
}

function printFormatMsg() {
    var msg = sprintf.apply(undefined, arguments);
    document.writeln(msg + "<br/>");
}

function printError(error) {
    printline("!!! Error name='" + error.name + ", message='" + error.message + "'");
}

// Asynchronously load and execute a script from a specified URL
function loadasync(url) {
    var head = document.getElementsByTagName("head")[0]; // Find document <head>
    var s = document.createElement("script"); // Create a <script> element
    s.src = url; // Set its src attribute 
    head.appendChild(s); // Insert the <script> into head
}