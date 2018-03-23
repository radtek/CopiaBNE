/// <reference path="stateChat.js" />
/// <reference path="~/UserControls/js/knockout-3.0.0.debug.js" />
/// <reference path="~/UserControls/js/linq-vsdoc.js" />
/// <reference path="~/UserControls/RenderizarChat.ascx" />
var MainViewModel = function () {
    var self = this;
    self.isConnected = ko.observable("");
    self.historyLoaded = ko.observable(false);
    var letterChatCount = 0;

    function findNextIndex() {
        return 1;
    }

    function cleanUpChat(cleanUpObj, originalObj) {
        cleanUpObj.moreInfo = null;
        cleanUpObj.allMessages = null;
        cleanUpObj.title = originalObj.title();
        cleanUpObj.maximized = originalObj.maximized();
        cleanUpObj.accumulated = originalObj.accumulated();

        return cleanUpObj;
    }

    var chatViewModel = function (guid, title, index, maximized, accumulated, targetId) {
        var chat = this;
        var messageCount = 0;

        function findNextletter() {
            var letter = codeToChar(letterChatCount);
            letterChatCount = letterChatCount + 1;
            return letter;
        }

        function saveStateObject() {
            var chatParsedToClone = cleanUpChat(clone(chat), chat);
            var chatKey = "Chat|" + chat.guid.toString();

            $.jStorage.set(chatKey, chatParsedToClone, { TTL: msStateTime });
        }
        chat.userFocus = ko.observable(false);
        chat.historyFilled = ko.observable(false);
        chat.takeFocus = ko.observable(false);
        chat.takeFocus.subscribe(function () {
            if (!chat.takeFocus()) {
                return;
            }
            if (chat.maximized())
                return;

            chat.maximized(true);
        });
        chat.letter = findNextletter();
        chat.guid = isUndefined(guid) ? generateGuid() : guid;
        chat.title = ko.observable(title);
        chat.index = isUndefined(index) ? findNextIndex() : index;
        chat.textToSend = ko.observable("");
        chat.targetId = isUndefined(targetId) ? 0 : targetId;
        chat.activeBlink = ko.observable(false);
        chat.linkTargetId = function () {
            if (window.location.port != '80') {
                return "http://" + window.location.hostname + ':' + window.location.port + pathRenderLink + "?targetId=" + chat.targetId;
            }
            return "http://" + window.location.hostname + pathRenderLink + "?targetId=" + chat.targetId;
        };

        chat.accessTargetLink = function () {
            window.open(chat.linkTargetId(), "_blank");
        };

        chat.showMoreInfo = ko.observable(false);

        chat.activeThisChat = function () {
            self.allChats.remove(chat);
            self.allChats.push(chat);
            chat.takeFocus(true);
        };

        chat.changeMoreInfoVisibility = function (obj) {
            obj.showMoreInfo(!obj.showMoreInfo());
        };

        chat.changeMaximizedStatus = function (obj) {
            obj.maximized(!obj.maximized());
        };

        chat.closeActiveChat = function (obj) {
            self.allChats.remove(obj);
            obj.activeBlink(false);

            if (!isUndefined(chatConnection))
                chatConnection.sendCloseChatToServer(cleanUpChat(clone(chat), chat));

            var last = Enumerable.From(self.activeChats.validItems()).FirstOrDefault(null, function (act) {
                return act.maximized();
            });

            if (last == null)
                return;

            last.takeFocus(true);
        };

        chat.closeExceededChat = function (obj) {
            self.allChats.remove(obj);
        };

        chat.maximized = ko.observable((isUndefined(maximized) ? true : maximized));
        chat.maximized.subscribe(function () {
            saveStateObject();

            if (chat.maximized()) {
                chat.takeFocus(true);
            }
        });

        chat.accumulated = ko.observable(isUndefined(accumulated) ? false : accumulated);
        chat.accumulated.subscribe(function () {
            saveStateObject();
        });

        chat.allMessages = ko.observableArray([]);

        function cleanUpMessage(cleanUpObj, originalObj) {
            cleanUpObj.content = originalObj.content();
            cleanUpObj.owner = originalObj.owner();
            cleanUpObj.guid = originalObj.guid();

            return cleanUpObj;
        }

        var messageViewModel = function (messageGuid, content, owner, receiveDate, sendDate, statusMessage, statusDate, errorToSend) {
            var message = this;
            messageCount = messageCount + 1;
            message.counter = messageCount;

            message.guid = isUndefined(messageGuid) ? ko.observable(generateGuid()) : ko.observable(messageGuid);
            message.content = isUndefined(content) ? ko.observable("") : ko.observable(content);
            message.owner = isUndefined(owner) ? ko.observable(false) : ko.observable(owner);

            if (owner) {
                message.sendDate = ko.observable(isUndefined(sendDate) ? new Date() : isDateOrParseOrDefault(sendDate));
                message.statusDate = ko.observable(isUndefined(statusDate) ? new Date() : isDateOrParseOrDefault(sendDate));
                message.statusMessage = ko.observable(isUndefined(statusMessage) ? 0 : statusMessage);
                message.errorToSend = ko.observable(isUndefined(errorToSend) ? false : errorToSend);
                message.receiveDate = ko.observable(new Date());
            }
            else {
                message.sendDate = ko.observable(new Date());
                message.statusDate = ko.observable(new Date());
                message.errorToSend = ko.observable(false);
                message.statusMessage = ko.observable(0);
                message.receiveDate = ko.observable(isUndefined(receiveDate) ? new Date() : isDateOrParseOrDefault(receiveDate));
            }

            message.thumbTargetImage = function () {
                if (chat.targetId <= 0)
                    return pathRenderImg + "/person.png";
                if (!message.owner) {
                    return pathRenderImg + "/person.png";
                }
                return pathRenderThumb + "?targetId=" + chat.targetId;
            };
        };

        chat.sendMessage = function (obj, content) {

            if (chat.activeBlink())
                chat.activeBlink(false);

            if (content == null
                || content.toString().trim() == "")
                return;

            var trySendMessage = self.isConnected() == 'Y';
            var newMessage;
            if (trySendMessage) {
                newMessage = new messageViewModel(null, content, true, null, new Date(), -1);
                obj.textToSend("");
            } else {
                newMessage = new messageViewModel(null, content, true, null, new Date(), 6, null, true);
            }

            chat.allMessages.push(newMessage);

            if (isUndefined(chatConnection)) {
                newMessage.statusMessage(6);
                newMessage.errorToSend(true);
            } else {

                var messageDto = cleanUpMessage(clone(newMessage), newMessage);

                var contact = Enumerable.From(self.onlineContacts.validItems()).FirstOrDefault(null, function (s) {

                    if (s.guid instanceof Function) {
                        if (chat.guid instanceof Function)
                            return s.guid() == chat.guid();
                        else
                            return s.guid() == chat.guid;
                    } else {
                        if (chat.guid instanceof Function)
                            return s.guid == chat.guid();
                        else
                            return s.guid == chat.guid;
                    }
                });

                if (contact != null) {
                    contact.lastMessage(messageDto.content);
                    contact.lastDate(new Date());
                }

                chatConnection.sendMessageToServer({ message: messageDto, chat: chat }, function (doneResult) {
                    if (isUndefined(doneResult)
                        || isUndefined(doneResult.Result)) {
                        return;
                    }

                    var resultMessageDto = doneResult.Result;

                    if (!isUndefined(resultMessageDto.Id))
                        newMessage.guid(resultMessageDto.Id);

                    if (isUndefined(resultMessageDto.Status))
                        newMessage.statusMessage(resultMessageDto);
                    else
                        newMessage.statusMessage(resultMessageDto.Status);

                }, function (fail) {
                    newMessage.statusMessage(6);
                    console.write(fail);
                });
            }

        };

        function createMessage(owner) {
            var newMessage = new messageViewModel(null, null, owner);
            chat.allMessages.push(newMessage);
            return newMessage;
        };

        chat.createMessage = function (owner) {
            return createMessage(owner);
        };

        chat.addMessage = function (chatDto, addUnshift) {
            if (Enumerable.From(chat.allMessages.validItems()).Any(function (obj) {
                if (obj.guid instanceof Function)
                    return obj.guid() == chatDto.guid;

                return obj.guid == chatDto.guid;
            })) {
                return false;
            }

            var messageVm = new messageViewModel(chatDto.guid, chatDto.content, chatDto.owner, chatDto.receiveDate, chatDto.sendDate, chatDto.statusMessage, chatDto.statusDate, false);
            if (isUndefined(addUnshift) || !addUnshift)
                chat.allMessages.push(messageVm);
            else
                chat.allMessages.unshift(messageVm);

            return true;
        };
        if (!boolIsLoadingState)
            saveStateObject();

        chat.detailsLoaded = ko.observable(false);
        chat.getMoreInfo = function () {
            if (!chat.showMoreInfo())
                return;

            if (chat.detailsLoaded())
                return;

            chatConnection.requestMoreInfo(cleanUpChat(clone(chat), chat), function (res) {

                if (isUndefined(res)
                    || isUndefined(res.Result)) {
                    chat.details("Não disponível.");
                    return;
                }


                chat.details(res.Result);
                chat.detailsLoaded = ko.observable(true);

            }, function (fail) {
                chat.details("Não disponível.");
                return;
            });
        };

        chat.details = ko.observable("");
        chat.cloneToDto = function () {
            return cleanUpChat(clone(chat), chat)
        };
    };

    var chatManager = ko.observableArray([]);

    self.activeChats = ko.observableArray([]);
    self.accumulatedChats = ko.observableArray([]);

    self.allChats = chatManager;

    self.balanceChats = function () {
        if (self.accumulatedChats.totalVisible() <= 0) {
            return;
        }

        if (self.activeChats.totalVisible() < maxLength) {
            var swapElement = self.accumulatedChats.validItems()[self.accumulatedChats.totalVisible() - 1];

            self.accumulatedChats.remove(swapElement);
            self.activeChats.unshift(swapElement);
            if (!swapElement.maximized()) {
                swapElement.maximized(true);
            }
            swapElement.accumulated(false);
            swapElement.takeFocus(true);
        }
    };

    self.allChats.subscribe(function (changes) {
        changes.forEach(function (entry) {
            if (entry.status == 'added') {
                self.activeChats.push(entry.value);
                if (boolIsLoadingState)
                    return;

                if (!entry.value.maximized()) {
                    entry.value.maximized(true);
                }
                entry.value.accumulated(false);
            } else if (entry.status == 'deleted') {

                self.accumulatedChats.remove(entry.value);
                self.activeChats.remove(entry.value);

                if (self.activeChats.totalVisible() == (maxLength - 1)
                    && self.accumulatedChats.totalVisible() > 0) {
                    self.balanceChats();
                }
            }

            if (!boolIsLoadingState) {
                self.saveState();
            }
        });
    }, null, "arrayChange");
    self.activeChats.subscribe(function (changes) {
        changes.forEach(function (entry) {
            if (entry.status == 'added') {
                if (self.activeChats.totalVisible() > maxLength) {

                    var removeElement = self.activeChats.validItems()[0];
                    self.accumulatedChats.push(removeElement);
                    removeElement.accumulated(true);
                    self.activeChats.splice(0, 1);
                }
            } else if (entry.status == 'deleted') {
                self.balanceChats();
            }
        });
    }, null, "arrayChange");

    function cleanUpContact(cleanUpObj, originalObj) {
        cleanUpObj.notification = originalObj.notification();
        return cleanUpObj;
    }

    var contactViewModel = function (guid, targetId, title, lastDate, lastMessage, ownerOfLastMessage, notification) {
        var contact = this;
        contact.guid = isUndefined(guid) ? generateGuid() : guid;
        contact.targetId = isUndefined(targetId) ? 0 : targetId;
        contact.title = ko.observable(isUndefined(title) ? '' : title);
        contact.lastMessage = ko.observable(isUndefined(lastMessage) ? '' : lastMessage);
        contact.lastDate = ko.observable(isUndefined(lastDate) ? null : isDateOrParseOrDefault(lastDate));
        contact.ownerOfLastMessage = ko.observable(isUndefined(ownerOfLastMessage) ? false : ownerOfLastMessage);
        contact.notification = ko.observable(isUndefined(notification) ? false : notification);
        contact.mouseOver = ko.observable(false);
        contact.overEvent = function () {
            this.mouseOver(!this.mouseOver())
        };
        contact.thumbTargetImage = function () {
            if (contact.targetId <= 0)
                return pathRenderImg + "/person.png";

            return pathRenderThumb + "?targetId=" + contact.targetId;
        };

        contact.notificationDate = ko.computed(function () {
            if (contact.lastDate() == null) {
                return "";
            } else {
                return translateMessageDate(new Date(), contact.lastDate());
            }
        });

        contact.openChat = function () {
            var started = self.tryStartNewChat(contact);

            if (contact.notification()) {
                var toFind = Enumerable.From(self.activeChats.validItems());
                var search = toFind.FirstOrDefault(null, function (obj) {
                    return obj.guid == contact.guid || (obj.targetId != 0 && obj.targetId == contact.targetId);
                });

                if (search != null) {
                    self.trySendReadNotification(search);
                }
            }

            if (!started)
                return;

            var requestHist = {
                guid: contact.guid,
                targetId: contact.targetId
            };

            // todo, test if exists, refactoring, uncouple functions (duplicate code) and handle exceptions
            chatConnection.requestHistory([requestHist], function (res) {
                if (isUndefined(res)
                    || res.length == 0
                    || isUndefined(res.Result)
                    || res.Result.length == 0)
                    return;

                if (!(res.Result instanceof Array)) {
                    //todo
                    return;
                }

                res.Result.forEach(function (obj) {

                    var messageDto = chatConnection.convertToMessageDto(obj);
                    var chatInfoDto = chatConnection.convertToChatDto(obj);

                    scopeViewModel.addReceivedMessage(messageDto, chatInfoDto, false, true);
                });

            });
        };

        contact.deleteContact = function () {
            self.deleteContact(contact);
        };

        contact.cloneToDto = function () {
            return cleanUpContact(clone(contact), contact)
        };

    };
    self.groupContacts = ko.observableArray([]);
    self.onlineContacts = ko.sortedObservableArray(function (a, b) {
        var a1 = a.lastDate();
        var b1 = b.lastDate();

        if (isUndefined(a1) || isUndefined(b1)) {
            if (isUndefined(a1) && isUndefined(b1))
                return 0;
            if (isUndefined(b1))
                return 1;
            return -1;
        };

        return b1.getTime() - a1.getTime();
    }, []);
    self.onlineContacts.subscribe(function (changes) {
        if (self.onlineContacts.totalVisible() == 1) {
            self.showContactList.notifySubscribers();
        }

        if (self.onlineContacts.totalVisible() <= 0) {
            self.groupContacts.removeAll();
            return;
        }

        var currentOnline = Enumerable.From(self.onlineContacts.validItems());
        // todo grouping contacts
    });

    self.tryStartNewChat = function (contactInstance) {
        if (isUndefined(contactInstance)) {
            return false;
        }

        var toFind = Enumerable.From(self.activeChats.validItems());
        var search = toFind.FirstOrDefault(null, function (obj) {
            return obj.guid == contactInstance.guid || (obj.targetId != 0 && obj.targetId == contactInstance.targetId);
        });

        if (search != null) {
            search.takeFocus(true);
            return false;
        }

        toFind = Enumerable.From(self.accumulatedChats.validItems());
        var selection = toFind.FirstOrDefault(null, function (obj) {
            return obj.guid == contactInstance.guid || (obj.targetId != 0 && obj.targetId == contactInstance.targetId);
        });

        if (selection != null) {
            selection.activeThisChat();
            return false;
        }

        var newChat = new chatViewModel(contactInstance.guid, contactInstance.title(), null, true, false, contactInstance.targetId);
        newChat.userFocus.subscribe(function (changes) {
            if (newChat.userFocus()) {
                self.trySendReadNotification(newChat);
            }
        });

        self.allChats.push(newChat);
        newChat.takeFocus(true);
        return true;
    };

    var contactListMaximizedKey = "ContactListMaximized";
    self.loadMaximizeDefinition = function () {
        var res = $.jStorage.get(contactListMaximizedKey);
        if (isUndefined(res))
            return true;

        return res;
    };

    self.contactListMaximized = ko.observable(self.loadMaximizeDefinition());
    self.contactListMaximized.subscribe(function () {
        $.jStorage.set(contactListMaximizedKey, self.contactListMaximized());
    });

    self.changeContactListMaximizedStatus = function () {
        self.contactListMaximized(!self.contactListMaximized());
    };

    self.createEmptyContact = function () {
        var newContact = new contactViewModel();
        self.onlineContacts.push(newContact);
        return newContact;
    };
    self.createEmptyChat = function () {
        var newChat = new chatViewModel();
        self.allChats.push(newChat);
        return newChat;
    };

    var windowStateKey = "WindowsInChat";
    self.saveState = function () {
        var toSave = [];
        self.allChats.validItems().forEach(function (obj) {
            var chatKey = "Chat|" + obj.guid.toString();
            toSave.push(chatKey);
        });

        $.jStorage.set(windowStateKey, toSave, { TTL: msStateTime });

    };
    self.clearCacheHistory = function () {
        $.jStorage.deleteKey(windowStateKey);
    };
    self.bringHistory = function () {
        var chatItems = $.jStorage.get(windowStateKey);

        if (chatItems == null
            || chatItems == typeof undefined) {
            return [];
        }

        boolIsLoadingState = true;
        var toPopulate = [];
        chatItems.forEach(function (obj) {
            var chatInfo = $.jStorage.get(obj);

            if (chatInfo == null
                || chatInfo == typeof (undefined))
                return;

            var chatObj = new chatViewModel(chatInfo.guid, chatInfo.title, chatInfo.index, chatInfo.maximized, chatInfo.accumulated, chatInfo.targetId);
            chatObj.userFocus.subscribe(function (changes) {
                if (chatObj.userFocus()) {
                    self.trySendReadNotification(chatObj);
                }
            });

            var contact = new contactViewModel(chatInfo.guid, chatInfo.targetId, chatInfo.title, null, '');

            self.onlineContacts.push(contact);
            toPopulate.push(chatObj);
        });

        toPopulate.sort(function (a, b) {
            if (a.accumulated()) {
                if (b.accumulated()) {
                    return 0;
                }
                return -1;
            } else {
                if (b.accumulated()) {
                    return 1;
                }
                return 0;
            }

        }).forEach(function (obj) {
            self.allChats.push(obj);
        });

        boolIsLoadingState = false;
        return toPopulate;
    };

    self.addReceivedMessage = function (messageDto, chatInfoDto, addUnshift, fromHistory) {

        var validChats = Enumerable.From(self.allChats.validItems());
        var persistentChat = validChats.FirstOrDefault(null, function (s) {
            return s.guid == chatInfoDto.guid;
        });
        var pendingTask = null;

        if (persistentChat == null) {
            persistentChat = validChats.FirstOrDefault(null, function (s) {
                return s.targetId == chatInfoDto.targetId;
            });

            if (persistentChat == null) {
                persistentChat = new chatViewModel(chatInfoDto.guid, chatInfoDto.title, null, true, false, chatInfoDto.targetId);
                persistentChat.userFocus.subscribe(function (changes) {
                    if (persistentChat.userFocus()) {
                        self.trySendReadNotification(persistentChat);
                    }
                });
                self.allChats.push(persistentChat);

                var requestHist = {
                    guid: chatInfoDto.guid,
                    targetId: chatInfoDto.targetId
                };

                // todo, test if exists, refactoring, uncouple functions (duplicate code) and handle exceptions
                pendingTask = function () {
                    chatConnection.requestHistory([requestHist], function (res2) {
                        if (isUndefined(res2)
                            || res2.length == 0
                            || isUndefined(res2.Result)
                            || res2.Result.length == 0)
                            return;

                        if (!(res2.Result instanceof Array)) {
                            //todo
                            return;
                        }

                        var sorted = [];

                        res2.Result.forEach(function (obj) {

                            var messageDtoRes = chatConnection.convertToMessageDto(obj);
                            var chatInfoDtoRes = chatConnection.convertToChatDto(obj);

                            messageDtoRes.getCreateDate = function () {
                                var current = this;

                                if (current.owner) {
                                    return new Date(current.sendDate);
                                } else {
                                    return new Date(current.receiveDate);
                                }
                            };
                            sorted.push({ m: messageDtoRes, c: chatInfoDtoRes });

                        });

                        sorted.sort(function (a, b) {
                            if (a.m.getCreateDate().getTime() == b.m.getCreateDate().getTime()) {
                                return 0;
                            } else {
                                if (a.m.getCreateDate().getTime() > b.m.getCreateDate().getTime()) {
                                    return -1;
                                } else {
                                    return 1;
                                }
                            }
                        });

                        sorted.forEach(function (item) {
                            scopeViewModel.addReceivedMessage(item.m, item.c, true, true);
                        });
                    });
                }
            }
        }

        if (!persistentChat.historyFilled())
            persistentChat.historyFilled(true);

        var added = persistentChat.addMessage(messageDto, addUnshift);

        try {
            if (boolIsLoadingState)
                return;

            if (addUnshift)
                return;

            if (messageDto.owner) {
                self.tryAddOrUpdateContact(new contactViewModel(chatInfoDto.guid, chatInfoDto.targetId,
                                                                chatInfoDto.title, messageDto.sendDate,
                                                                messageDto.content, true, fromHistory ? chatInfoDto.notification : added));
            } else {
                self.tryAddOrUpdateContact(new contactViewModel(chatInfoDto.guid, chatInfoDto.targetId,
                                                                chatInfoDto.title, messageDto.receiveDate,
                                                                messageDto.content, false, fromHistory ? chatInfoDto.notification : added));
            }

            if (!messageDto.owner && !fromHistory) {
                if (!persistentChat.userFocus())
                    persistentChat.activeBlink(true);
            }
        }
        finally {
            if (pendingTask != null) {
                pendingTask();
            }
        }
    };

    self.updateMessageStatus = function (statusDto, chatInfoDto) {
        var validChats = Enumerable.From(self.allChats.validItems());
        var persistentChat = validChats.FirstOrDefault(null, function (s) {
            if (s.guid instanceof Function) {
                return s.guid() == chatInfoDto.guid;
            }
            return s.guid == chatInfoDto.guid;
        });

        var toFindMessage;
        if (persistentChat == null) {
            persistentChat = validChats.FirstOrDefault(null, function (s) {
                if (s.targetId instanceof Function)
                    return s.targetId() == chatInfoDto.targetId;

                return s.targetId == chatInfoDto.targetId;
            });

            if (persistentChat == null) {
                toFindMessage = validChats.SelectMany(function (s) {
                    return Enumerable.From(s.allMessages.validItems());
                });
            } else {
                toFindMessage = Enumerable.From(persistentChat.allMessages.validItems());
            }
        } else {
            toFindMessage = Enumerable.From(persistentChat.allMessages.validItems());
        }

        var messageVm = toFindMessage.FirstOrDefault(null, function (s) {
            if (s.guid instanceof Function)
                return s.guid() == statusDto.guid;

            return s.guid == statusDto.guid;
        });

        if (messageVm != null) {
            messageVm.statusMessage(statusDto.statusMessage);
            messageVm.statusDate(statusDto.statusDate);
        }
    };

    self.onHistoryLoaded = function () {
        self.historyLoaded(true);

        var updateLastMessage = Enumerable.From(self.onlineContacts.validItems());
        self.allChats.validItems().forEach(function (item) {

            var visible = item.allMessages.totalVisible();
            if (visible == 0)
                return;

            var lastMessage = item.allMessages.validItems()[visible - 1];
            var res = updateLastMessage.FirstOrDefault(null, function (find) {
                return find.guid == item.guid || (find.targetId > 0 && find.targetId == item.targetId);
            });

            if (res == null)
                return;

            res.lastMessage(lastMessage.content());

            if (lastMessage.owner instanceof Function ? lastMessage.owner() : lastMessage.owner) {
                res.lastDate(lastMessage.sendDate());
            } else {
                res.lastDate(lastMessage.receiveDate());
            }
        });
    };

    self.loadMaximizeDefinition();

    self.showContactList = ko.computed(function () {

        if (self.activeChats.totalVisible() > 0
            || self.onlineContacts.totalVisible() > 0)
            return true;

        return false;
    });

    self.addNewOnlineContact = function (contactInfo) {
        if (isUndefined(contactInfo))
            return;

        var contact = new contactViewModel(contactInfo.guid, contactInfo.targetId, contactInfo.title, contactInfo.lastDate, contactInfo.lastMessage, contactInfo.owner, contactInfo.notification);
        self.tryAddOrUpdateContact(contact);
    };

    self.tryAddOrUpdateContact = function (contactVm) {
        if (isUndefined(contactVm))
            return;

        var allContacts = Enumerable.From(self.onlineContacts.validItems());

        var guidToUse = contactVm.guid instanceof Function ? contactVm.guid() : contactVm.guid;
        var targetIdToUse = contactVm.targetId instanceof Function ? contactVm.targetId() : contactVm.targetId;

        var existent = allContacts.FirstOrDefault(null, function (obj) {
            var targetToCompare = obj.targetId instanceof Function ? obj.targetId() : obj.targetId;
            return (obj.guid instanceof Function ? obj.guid() : obj.guid) == guidToUse || (targetToCompare != 0 && targetToCompare == targetIdToUse);
        });

        if (contactVm.notification() && contactVm.ownerOfLastMessage()) {
            contactVm.notification(false);
        }

        if (existent == null) {
            self.onlineContacts.push(contactVm);
            return;
        }

        if (existent.lastDate() != null) {
            if (existent.lastMessage() != '...' && existent.lastDate().getTime() >= contactVm.lastDate().getTime()) {
                return;
            }
        }

        existent.lastMessage(contactVm.lastMessage());
        existent.lastDate(contactVm.lastDate());
        existent.ownerOfLastMessage(contactVm.ownerOfLastMessage());

        if (contactVm.notification()) {
            var activeWindow = self.activeChats.validItems();

            var findActiveChat = Enumerable.From(activeWindow).FirstOrDefault(null, function (obj) {
                if (!obj.userFocus()) {
                    return false;
                }

                var targetToCompare = obj.targetId instanceof Function ? obj.targetId() : obj.targetId;
                return (obj.guid instanceof Function ? obj.guid() : obj.guid) == guidToUse || (targetToCompare != 0 && targetToCompare == targetIdToUse);
            });

            if (findActiveChat == null) {
                existent.notification(true);
            }
            else {
                self.trySendReadNotification(findActiveChat);
            }
        }
        else {
            existent.notification(false);
        }
        self.onlineContacts.remove(existent);
        self.onlineContacts.push(existent);
    };

    self.closeAllChats = function () {
        var items = self.allChats.validItems();

        for (var i = 0; i < items.length; i++) {
            self.allChats.remove(items[i]);
        }
    };

    self.trySendReadNotification = function (chatWindow) {
        var guidToUse = (chatWindow.guid instanceof Function) ? chatWindow.guid() : chatWindow.guid;
        var targetIdToUse = (chatWindow.targetId instanceof Function) ? chatWindow.targetId() : chatWindow.targetId;

        var contactItems = Enumerable.From(self.onlineContacts.validItems());

        var res = contactItems.FirstOrDefault(null, function (obj) {
            if (!obj.notification())
                return false;

            var guidToCompare = (obj.guid instanceof Function) ? obj.guid() : obj.guid;
            var targetIdToCompare = (obj.targetId instanceof Function) ? obj.targetId() : obj.targetId;

            if ((guidToUse == guidToCompare && guidToCompare != 0) || (targetIdToUse == targetIdToCompare && targetIdToCompare != 0))
                return true;

            return false;
        });

        if (res != null) {
            self.sendReadNotification(chatWindow);
            res.notification(false);
            chatWindow.activeBlink(false);
        }
    };
    self.sendReadNotification = function (chatWindow) {
        chatConnection.sendReadNotificationInConversation(chatWindow.cloneToDto(), function (res) {
            return;
        }, function (fail) {
            return;
        });
    };

    self.deleteContact = function (contactVm) {
        self.onlineContacts.remove(contactVm);

        var validChats = Enumerable.From(self.allChats.validItems());
        var persistentChat = validChats.FirstOrDefault(null, function (s) {
            return getValue(s.guid) == getValue(contactVm.guid) || (getValue(s.targetId) > 0 && getValue(s.targetId) == getValue(contactVm.targetId));
        });
        if (persistentChat != null) {
            self.allChats.remove(persistentChat);
        }

        var chatKey = "Chat|" + contactVm.guid.toString();
        $.jStorage.deleteKey(chatKey);

        chatConnection.sendDeleteContact(contactVm.cloneToDto());
    };

    self.receiveRemoveContact = function (toRemove) {

        var validChats = Enumerable.From(self.allChats.validItems());
        var persistentChat = validChats.FirstOrDefault(null, function (s) {
            return getValue(s.targetId) > 0 && getValue(s.targetId) == getValue(toRemove);
        });
        if (persistentChat != null) {
            self.allChats.remove(persistentChat);
        }

        var validContacts = Enumerable.From(self.onlineContacts.validItems());
        var persistentContact = validContacts.FirstOrDefault(null, function (s) {
            return getValue(s.targetId) > 0 && getValue(s.targetId) == getValue(toRemove);
        });

        if (persistentChat != null) {
            self.onlineContacts.remove(persistentContact);
        }

    }
}

function getValue(arg)
{
    if (arg instanceof Function)
        return arg();

    return arg;
}