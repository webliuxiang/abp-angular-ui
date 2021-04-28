var app = app || {};
(function () {

    $.extend(app, {
        abpPro: {
            
            aliyunPlayerConfig: {
                "id": "L52abp-CoursePlay",
                "width": "100%",
                "height": "768px",
                "autoplay": false,
                "source": "//player.alicdn.com/video/aliyunmedia.mp4",
                //vid: videoInfo.videoMeta.videoId,
                //playauth: videoInfo.playAuth,
                cover: "http://oss.52abp.com/58077cc00532428ebabdb06fb1f90b31/snapshots/7bc41e403e994233a21cdcd5d872e421-00005.jpg",

                "isLive": false,
                "rePlay": false,
                "playsinline": true,
                "preload": true,
                "autoPlayDelay": "2",
                "autoPlayDelayDisplayText": "视频加载中....",
                "loadDataTimeout": "30",
                "language": "zh-cn",
                "controlBarVisibility": "hover",
                "useH5Prism": true,
                encryptType: true,
                "skinLayout": [
                    {
                        "name": "bigPlayButton",
                        "align": "blabs",
                        "x": 30,
                        "y": 80
                    },
                    {
                        "name": "H5Loading",
                        "align": "cc"
                    },
                    {
                        "name": "errorDisplay",
                        "align": "tlabs",
                        "x": 0,
                        "y": 0
                    },
                    {
                        "name": "infoDisplay"
                    },
                    {
                        "name": "tooltip",
                        "align": "blabs",
                        "x": 0,
                        "y": 56
                    },
                    {
                        "name": "thumbnail"
                    },
                    {
                        "name": "controlBar",
                        "align": "blabs",
                        "x": 0,
                        "y": 0,
                        "children": [
                            {
                                "name": "progress",
                                "align": "blabs",
                                "x": 0,
                                "y": 44
                            },
                            {
                                "name": "playButton",
                                "align": "tl",
                                "x": 15,
                                "y": 12
                            },
                            {
                                "name": "timeDisplay",
                                "align": "tl",
                                "x": 10,
                                "y": 7
                            },
                            {
                                "name": "fullScreenButton",
                                "align": "tr",
                                "x": 10,
                                "y": 12
                            },
                            {
                                "name": "setting",
                                "align": "tr",
                                "x": 15,
                                "y": 12
                            },
                            {
                                "name": "volume",
                                "align": "tr",
                                "x": 5,
                                "y": 10
                            }
                        ]
                    }
                ]
            }
           
        }
    });

})();