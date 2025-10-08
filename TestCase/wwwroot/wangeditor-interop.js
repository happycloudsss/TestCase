window.wangEditorInterop = {
    instances: {},

    initialize: async function (instanceId, dotNetHelper, toolbarId, editorId, initialContent, options) {
        // 确保wangEditor库已加载
        if (!window.wangEditor) {
            console.error('wangEditor library not loaded');
            return;
        }

        const { createEditor, createToolbar } = window.wangEditor;

        // 销毁已存在的同名实例
        this.destroy(instanceId);

        // 编辑器配置
        const editorConfig = {
            placeholder: options.placeholder || '请输入内容...',
            mode: 'default',
            readOnly: options.readOnly || false,
            onChange: (editor) => {
                const html = editor.getHtml();
                try {
                    dotNetHelper.invokeMethodAsync('OnContentChanged', html);
                } catch (error) {
                    // 忽略组件已被销毁的情况
                    if (!error.message.includes('has been disposed')) {
                        console.error('Error invoking OnContentChanged:', error);
                    }
                }
            }
        };

        // 如果配置了图片上传，则添加上传配置
        if (options.uploadImageApi) {
            editorConfig.MENU_CONF = {};
            editorConfig.MENU_CONF['uploadImage'] = {
                server: options.uploadImageApi,
                fieldName: 'file',
                // 上传之前触发
                onBeforeUpload(file) {
                    // console.log('上传之前', file)
                    return file; // 返回false可终止上传
                },
                // 上传进度触发
                onProgress(progress) {
                    // console.log('上传进度', progress)
                },
                // 上传成功后触发
                onSuccess(file, res) {
                    // console.log('上传成功', file, res)
                },
                // 上传失败后触发
                onError(file, res) {
                    // console.log('上传失败', file, res)
                    alert('图片上传失败: ' + (res?.message || '未知错误'));
                },
                // 自定义插入图片
                customInsert(res, insertFn) {
                    // console.log('customInsert', res)
                    // 从res中获取图片url，然后插入图片
                    if (res && res.url) {
                        insertFn(res.url, '', '')
                    }
                }
            };
        }

        // 创建编辑器实例
        let editor;
        try {
            editor = createEditor({
                selector: `#${editorId}`,
                config: editorConfig,
                html: initialContent || ''
            });
        } catch (error) {
            console.error('Failed to create editor:', error);
            return;
        }

        // 构建工具栏配置
        let toolbarKeys;
        if (options.customToolbarKeys && options.customToolbarKeys.length > 0) {
            // 使用自定义工具栏配置
            toolbarKeys = options.customToolbarKeys;
        }

        // 工具栏配置
        const toolbarConfig = {
            excludeKeys: ['group-video', 'fullScreen', 'insertImage']
        };

        if (toolbarKeys) {
            toolbarConfig.toolbarKeys = toolbarKeys;
        }


        // 创建工具栏
        let toolbar;
        try {
            toolbar = createToolbar({
                editor: editor,
                selector: `#${toolbarId}`,
                config: toolbarConfig
            });
        } catch (error) {
            console.error('Failed to create toolbar:', error);
        }

        // 存储实例引用
        this.instances[instanceId] = {
            editor: editor,
            toolbar: toolbar,
            dotNetHelper: dotNetHelper
        };
    },

    setContent: function (instanceId, content) {
        const instance = this.instances[instanceId];
        if (instance && instance.editor) {
            instance.editor.setHtml(content || '');
        }
    },

    getContent: function (instanceId) {
        const instance = this.instances[instanceId];
        if (instance && instance.editor) {
            return instance.editor.getHtml();
        }
        return '';
    },

    setReadOnly: function (instanceId, readOnly) {
        const instance = this.instances[instanceId];
        if (instance && instance.editor) {
            if (readOnly) {
                instance.editor.disable();
            } else {
                instance.editor.enable();
            }
        }
    },

    refresh: function (instanceId) {
        const instance = this.instances[instanceId];
        if (instance && instance.editor) {
            // wangEditor通常会自动处理刷新，这里可以添加特殊逻辑
            console.log(`Refreshing editor instance: ${instanceId}`);
        }
    },

    destroy: function (instanceId) {
        const instance = this.instances[instanceId];
        if (instance) {
            // 销毁编辑器
            if (instance.editor) {
                try {
                    instance.editor.destroy();
                } catch (e) {
                    console.warn(`Error destroying editor ${instanceId}:`, e);
                }
            }

            // 清理DotNet引用
            if (instance.dotNetHelper) {
                try {
                    instance.dotNetHelper.dispose();
                } catch (e) {
                    console.warn(`Error disposing dotNetHelper ${instanceId}:`, e);
                }
            }

            // 删除实例引用
            delete this.instances[instanceId];
        }
    },

    // 销毁所有实例
    destroyAll: function () {
        Object.keys(this.instances).forEach(instanceId => {
            this.destroy(instanceId);
        });
    }
};