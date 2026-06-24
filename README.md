# 写给所爱之人 · 博客

这是一个用 GitHub Pages + Jekyll 搭的个人博客，写文章只需要往 `_posts/` 里放 Markdown 文件。

## 一、第一次上线（只做一次）

1. 在 GitHub 新建仓库，名字必须是 **gta0814.github.io**，可见性选 **Public**。
2. 把这个文件夹里的所有文件上传到仓库（仓库页面 → Add file → Upload files → 把文件全选拖进去 → Commit）。
   - 注意要保留文件夹结构：`_posts` 文件夹要原样上传。
3. 打开仓库的 **Settings → Pages**，在 "Build and deployment" 里把 Source 选成 **Deploy from a branch**，Branch 选 **main / (root)**，保存。
4. 等 1～2 分钟，访问 **https://gta0814.github.io** 就能看到博客了。

## 二、以后怎么写新文章

在 `_posts/` 文件夹里新建一个文件，文件名格式必须是：

```
年-月-日-英文标题.md
```

例如 `2026-08-01-a-letter-to-shiyuan.md`。文件开头照抄这段（叫 front matter）：

```
---
layout: post
title: "你的中文标题"
date: 2026-08-01 09:00:00 +0800
---

正文从这里开始，用 Markdown 写就行。
```

提交（commit）之后，过一两分钟博客首页就会自动出现这篇新文章，不用做别的。

## 三、改博客名字 / 简介

打开 `_config.yml`，改最上面的 `title` 和 `description` 即可。

## 文件说明

- `_config.yml` —— 博客的配置（名字、简介、主题）
- `index.md` —— 首页
- `about.md` —— "关于"页面
- `_posts/` —— 所有文章放这里
- `README.md` —— 就是这份说明，不会显示在网站上
