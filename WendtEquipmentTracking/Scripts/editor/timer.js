$(function () {

    var Timer = function () {

        this.idleTime = 0;
        this.seconds;

        this.startTimer = function (seconds) {
            var $this = this;

            this.seconds = seconds;

            var idleInterval = setInterval(function () { $this.timerIncrement.call($this); }, 1000);

            //Zero the idle timer on mouse movement.
            $(window).mousemove(function (e) {
                $this.idleTime = 0;
            });
            $(window).keypress(function (e) {
                $this.idleTime = 0;
            });
            
        }

        this.timerIncrement = function () {
            this.idleTime = this.idleTime + 1;
            if (this.idleTime > this.seconds) {
                window.location.reload();
            }
        }
    }

    timer = new Timer();

});