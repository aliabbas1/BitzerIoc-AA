function Helper(app)
{
  this.application = app;
    //this.data = [1, 2, 3];  setting a non-primitive property
};
Helper.prototype.resolveUrl = function (url) { return url.replace("~/","/") };
