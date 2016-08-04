﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace CommonUtilities
{
    [Serializable]
    public sealed class Exception<TExceptionArgs> : Exception
        where TExceptionArgs : ExceptionArgs
    {
        private const String CArgs = "Args";
        private readonly TExceptionArgs _args;
        public readonly List<ExceptionInfo> InnerExceptionInfos = new List<ExceptionInfo>();

        public void AddInnerException(Exception ex, string otherInfo)
        {
            var info = ExceptionInfo.Create(ex);
            info.OtherInfo = otherInfo;
            InnerExceptionInfos.Add(info);
        }

        public TExceptionArgs Args { get { return this._args; } }


        public Exception(String message = null, Exception innerException = null)
            : this(null, message, innerException)
        {

        }

        public Exception(TExceptionArgs args, String message = null, Exception innerException = null)
            : base(message, innerException)
        {
            this._args = args;
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        private Exception(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this._args = (TExceptionArgs)info.GetValue(CArgs, typeof(TExceptionArgs));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(CArgs, this._args);
            base.GetObjectData(info, context);
        }

        public override string Message
        {
            get
            {
                var baseMessage = base.Message;
                return this._args == null ? baseMessage : baseMessage + "(" + this._args.Message + ")";
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Exception<TExceptionArgs>;
            if (other == null)
            {
                return false;
            }

            return Equals(this._args, other.Args) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Serializable]
    public class ExceptionInfo
    {
        public string ExceptionTypeName;
        public string Message;
        public string StackTrace;
        public string Machine;
        public string OtherInfo;
        public ExceptionInfo InnerExceptionInfos;

        public static ExceptionInfo Create(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            var info = new ExceptionInfo()
                       {
                           ExceptionTypeName = ex.GetType().FullName,
                           Message = ex.Message,
                           StackTrace = ex.StackTrace,
                           Machine = Environment.MachineName
                       };
            if (ex.InnerException != null)
            {
                info.InnerExceptionInfos = Create(ex.InnerException);
            }

            return info;
        }
    }

    [Serializable]
    public abstract class ExceptionArgs
    {
        public virtual String Message { get { return String.Empty; } }
    }


    [Serializable]
    public sealed class VariableNotFoundExceptionArgs : ExceptionArgs { }

    [Serializable]
    public sealed class MethodNotFoundExceptionArgs : ExceptionArgs { }

    [Serializable]
    public sealed class TypeNotFoundExceptionArgs : ExceptionArgs { }

    [Serializable]
    public sealed class JobTypeNotFoundExceptionArgs : ExceptionArgs { }

    [Serializable]
    public sealed class AttributeMissingExceptionArgs : ExceptionArgs { }


    [Serializable]
    public sealed class MethodInvokingExceptionArgs : ExceptionArgs { }
    [Serializable]
    public sealed class AssemblyNotFoundExceptionArgs : ExceptionArgs { }
    [Serializable]
    public sealed class WorkflowConfigLoadExceptionArgs : ExceptionArgs { }
    [Serializable]
    public sealed class AlreadyResitedExceptionArgs : ExceptionArgs { }
    [Serializable]
    public sealed class VmNotFoundExceptionArgs : ExceptionArgs { }
    [Serializable]
    public sealed class NoPermissionExceptionArgs : ExceptionArgs { }
    [Serializable]
    public sealed class ConvertException : ExceptionArgs { }

}
