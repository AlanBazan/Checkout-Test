var Navigation = {

    host: '',

    init: function(host) {
        this.host = host;
    },

    push: function({ pageName, parameters })
    {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", `${this.host}api/navigation` , true);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

        xhr.onreadystatechange = function () { 
            if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
                console.log("message sent");
            }
        }

        xhr.send(JSON.stringify({ pageName, parameters }));
    }
}