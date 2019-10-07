<template>
    <div class="wrapper">
        <div class="overlay" v-if="showArticle" @click="show"></div>

        <div class="feed-item" v-for="item in feedList" @click="showCleanArtilce(item.Link);">
            <div class="feed-img">
                <img class="article-img" v-bind:src="item.ImageUrl" />
            </div>
            <div class="feed-info">
                <h3>{{item.Title}}</h3>
                <div class="feed-description">{{item.Description}}</div>
                <div class="feed-author">{{item.Author}}</div>
                <div class="feed-date">{{item.PubDate}}</div>
            </div>
        </div>
        <div class="modal" v-if="showArticle">
            <button class="close" @click="show">x</button>
            <div class="modal-inner">
                <div v-if="!clutterFreeLoading">
                    <h2 class="modal-title">{{clutterFreeArticle.Title}}</h2>
                    <div class="modal-info">
                        <small>{{clutterFreeArticle.Author}}</small>
                        <small>{{clutterFreeArticle.Date_published}}</small>
                        <small>{{clutterFreeArticle.Category}}</small>
                    </div>

                    <div class="modal-content" v-html="clutterFreeArticle.Content"></div>
                </div>
                <div class="loading" v-else>Loading article ... </div>
            </div>
        </div>
    </div>
</template>


<script>
    import axios from "axios";

    export default {
        data() {
            return {
                feedList: [],
                showArticle: false,
                clutterFreeArticle: [],
                clutterFreeLoading: false
            }
        },

        created() {
            axios
                .get('http://localhost:63463/feed/Snakey/GetRssFeed')
                .then((response) => {
                    this.feedList = response.data;
                    for (let i = 0; i < this.feedList.length; i++) {
                        let mediaUrl = this.feedList[i].ImageUrl;
                        if (mediaUrl == "") {
                            this.feedList[i].ImageUrl = "https://dubsism.files.wordpress.com/2017/12/image-not-found.png?w=547"
                        } else {
                            var url = mediaUrl.substring(1, mediaUrl.length - 1).split(" ")[1];
                            var cleanUrl = url.substring(5, url.length - 1);
                            this.feedList[i].ImageUrl = cleanUrl;

                        }
                    }

                })
                .catch((err) => {
                    console.log(err);
                })
        },

        methods: {
            showCleanArtilce(requestUrl) {
                window.scrollTo(0, 0);
                this.showArticle = !this.showArticle;
                this.clutterFreeLoading = true;
                axios
                    .post("http://localhost:63463/feed/Snakey/GetCleanArticleFrom", {
                        url: requestUrl,
                    })
                    .then((response) => {
                        this.clutterFreeArticle = response.data;
                        this.clutterFreeLoading = false;
                    })
                    .catch((err) => {
                        console.log(err);
                        this.clutterFreeLoading = false;
                    })
            },

            show() {
                this.showArticle = !this.showArticle;
            }
        }
    }
</script>

<style>
    .wrapper {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
    }

    .feed-item {
        padding: 1em;
        box-shadow: 1px 1px 8px -4px rgba(0,0,0,0.54);
        cursor: pointer;
        width: 320px;
        margin: 10px;
        border-radius: 5px;
        position: relative;
    }

    .article-img {
        /*width: 320px;
        height: 270px;*/
        width: 100%;
        object-fit: cover;
    }

    .feed-info {
        margin-bottom: 15%;
    }

    .feed-date {
        position: absolute;
        bottom: 5px;
        display: flex;
        width: 90%;
        flex-direction: row-reverse;
        font-weight: 500;
        opacity: 0.6;
    }

    .close {
        position: absolute;
        right: 5px;
        top: 0px;
        background-color: transparent;
        border: none;
        font-size: 30px;
        opacity: 0.5;
        cursor: pointer;
    }

    img {
        width: 100%;
    }

    @media (max-width : 599px) {
        .article-img {
            width: 100%;
        }

        .modal {
            width: 75% !important;
        }
    }

    .loading {
        width: 100%;
        text-align: center;
        font-weight: 500;
        font-size: 18px;
    }



    /*--------modal-------------*/

    .overlay {
        position: fixed;
        z-index: 9998;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0,0,0,0.5);
    }

    .modal {
        padding: 20px 30px;
        position: absolute;
        width: 70%;
        z-index: 9999;
        background-color: #FFF;
        border-radius: 5px;
    }

    pre {
        overflow-x:scroll;
    }

    em {
        word-break:break-all;
    }

    iframe {
        width: 100% !important;
    }
</style>
