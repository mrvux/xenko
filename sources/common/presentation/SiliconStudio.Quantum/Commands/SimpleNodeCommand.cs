﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using SiliconStudio.ActionStack;

namespace SiliconStudio.Quantum.Commands
{
    public abstract class SimpleNodeCommand : NodeCommandBase
    {
        public sealed override object Execute(object currentValue, object parameter, out UndoToken undoToken)
        {
            UndoToken token;
            var newValue = Do(currentValue, parameter, out token);
            undoToken = new UndoToken(token.CanUndo, new TokenData(parameter, token));
            return newValue;
        }

        public sealed override object Undo(object currentValue, UndoToken undoToken, out RedoToken redoToken)
        {
            var tokenData = (TokenData)undoToken.TokenValue;
            redoToken = new RedoToken(tokenData.Parameter);
            return Undo(currentValue, tokenData.Token);
        }

        public sealed override object Redo(object currentValue, RedoToken redoToken, out UndoToken undoToken)
        {
            return Execute(currentValue, redoToken.TokenValue, out undoToken);
        }

        /// <inheritdoc/>
        protected abstract object Do(object currentValue, object parameter, out UndoToken undoToken);
        
        /// <inheritdoc/>
        protected abstract object Undo(object currentValue, UndoToken undoToken);
    }
}