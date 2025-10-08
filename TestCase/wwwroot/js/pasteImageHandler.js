window.TestCase = window.TestCase || {};
window.TestCase.PasteImageHandler = {
    instances: new Map(),

    initialize: function (dotNetHelper, element) {
        if (!element) return;

        // 处理粘贴事件
        const pasteHandler = async function (e) {
            try {
                if (!e.clipboardData || !e.clipboardData.items) {
                    console.warn('无法访问剪贴板数据');
                    return;
                }

                const items = e.clipboardData.items;
                for (let i = 0; i < items.length; i++) {
                    if (items[i].type.indexOf('image') !== -1) {
                        const blob = items[i].getAsFile();
                        if (blob) {
                            const arrayBuffer = await blob.arrayBuffer();
                            const bytes = new Uint8Array(arrayBuffer);

                            // 生成文件名
                            const now = new Date();
                            const extension = blob.type.split('/')[1] || 'png';
                            const fileName = 'pasted_' +
                                now.getFullYear() +
                                ('0' + (now.getMonth() + 1)).slice(-2) +
                                ('0' + now.getDate()).slice(-2) + '_' +
                                ('0' + now.getHours()).slice(-2) +
                                ('0' + now.getMinutes()).slice(-2) +
                                ('0' + now.getSeconds()).slice(-2) +
                                '.' + extension;

                            // 调用.NET方法
                            await dotNetHelper.invokeMethodAsync('HandlePastedImage',
                                fileName,
                                bytes,
                                blob.type);

                            e.preventDefault();
                            return;
                        }
                    }
                }
            } catch (error) {
                console.error('处理粘贴事件时出错:', error);
            }
        };

        // 添加焦点事件处理
        const focusHandler = function () {
            element._isFocused = true;
        };

        const blurHandler = function () {
            element._isFocused = false;
        };

        // 添加事件监听器
        element.addEventListener('paste', pasteHandler);
        element.addEventListener('focus', focusHandler);
        element.addEventListener('click', focusHandler);
        element.addEventListener('blur', blurHandler);

        // 保存引用以便稍后清理
        const handlers = {
            pasteHandler: pasteHandler,
            focusHandler: focusHandler,
            blurHandler: blurHandler
        };

        element._pasteImageHandlers = handlers;
        this.instances.set(element, handlers);
    },

    dispose: function (element) {
        if (element && element._pasteImageHandlers) {
            const handlers = element._pasteImageHandlers;

            element.removeEventListener('paste', handlers.pasteHandler);
            element.removeEventListener('focus', handlers.focusHandler);
            element.removeEventListener('click', handlers.focusHandler);
            element.removeEventListener('blur', handlers.blurHandler);

            delete element._pasteImageHandlers;
            this.instances.delete(element);
        }
    }
};