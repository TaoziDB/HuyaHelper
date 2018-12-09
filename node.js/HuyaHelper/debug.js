
var util = require('util');

var debug = exports = module.exports = {};

debug.is_debug = true;

debug.setDebug = function setDebug(is_debug) {
    debug.is_debug = is_debug;
}

debug.print = function print(format, ...params) {
    if (debug.is_debug) {
        var args = Array.prototype.slice.call(arguments);
        var num_args = arguments.length;
        //console.log("num_args = " + num_args);
        if (format.indexOf("%") == -1) {
            for (i = 0; i < args.length; i++) {
                util.print(args[i]);
            }
        }
        else {
            var text = util.format(format, params);
            util.print(text);
        }
        util.print("\n");
    }
}

debug.enter = function enter(funcName) {
    if (debug.is_debug) {
        console.log("-------------------------------------------");
        console.log(funcName + " enter.");
    }
}

debug.leave = function leave(funcName) {
    if (debug.is_debug) {
        console.log(funcName + " leave.");
        console.log("-------------------------------------------");
    }
}

function createDebug() {
    var debug = new Debug();
    return debug;
}
